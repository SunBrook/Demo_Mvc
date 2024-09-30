using Demo_Mvc.Common.Models;
using Demo_Mvc.Common.Tools;
using Demo_Mvc.Service.BgManager.Login;
using Demo_Mvc.Service.OfficialWebSite.Home;
using Demo_Mvc.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO.Compression;
using System.Text.Json.Serialization;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Demo_Mvc
{
    /// <summary>
    /// 启动类
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = true;
            });

            // 压缩
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes =
                ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "image/svg+xml", "image/png", "image/webp", "video/mp4", "video/quicktime", "application/font-woff" });
                //options.EnableForHttps = true; // 仅对HTTPS请求启用压缩
            });

            //针对不同的压缩类型，设置对应的压缩级别
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.SmallestSize;
            });

            // 添加日志
            services.AddLogging(opt => opt.AddLog4Net("log4net.config"));
            // 向服务集合中添加W3C日志记录
            services.AddW3CLogging(logging =>
            {
                // 记录所有可能的W3C字段
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.W3CLoggingFields.All;

                // 设置日志文件的大小限制为5MB
                logging.FileSizeLimit = 5 * 1024 * 1024;
                // 设置保留的文件数量限制为
                logging.RetainedFileCountLimit = 1000;
                // 设置日志文件名为"w3c_logs"
                logging.FileName = "w3c_logs";

                // 设置日志目录为应用程序域的基目录下的"w3c_logs"文件夹
                logging.LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "w3c_logs");
                // 设置刷新间隔为5秒
                logging.FlushInterval = TimeSpan.FromSeconds(5);
            });

            //services.AddControllersWithViews();

            // 设置MySQL
            services.AddDbContext<MyDbContext>(options => options.UseMySql(Configuration.GetConnectionString("MySQL"), ServerVersion.Parse("5.7"))); //MySqlServerVersion.LatestSupportedServerVersion

            // 设置Redis
            var redisConnectionString = Configuration.GetSection("RedisConnectionString:Connection")?.Value ?? string.Empty;
            var redisInstanceName = Configuration.GetSection("RedisConnectionString:InstanceName")?.Value ?? string.Empty;
            services.AddSingleton(new RedisCacheHelper(redisConnectionString, redisInstanceName));

            // 添加 Swagger 中间件
            services.AddSwaggerGen(c =>
            {
                //Bearer 的scheme定义
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    //参数添加在头部
                    In = ParameterLocation.Header,
                    //使用Authorize头部
                    Type = SecuritySchemeType.Http,
                    //内容为以 bearer开头
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                //把所有方法配置为增加bearer头部信息
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth"
                            }
                        },
                        new string[] {}
                    }
                };

                //注册到swagger中
                c.AddSecurityDefinition("bearerAuth", securityScheme);
                c.AddSecurityRequirement(securityRequirement);
            });
            services.AddSwaggerSetup();

            // 为不同角色创建策略
            services.AddAuthorization(option =>
            {
                option.AddPolicy("accountSystem", policy => policy.RequireRole("Account", "Employee"));
                option.AddPolicy("Permission", policy => policy.Requirements.Add(new PolicyRequirement()));
            });

            // 添加 jwt 认证
            JwtConfig.Instance = Configuration.GetSection(nameof(JwtConfig))?.Get<JwtConfig>() ?? new JwtConfig();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,//是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(30),

                    ValidateAudience = true,//是否验证Audience
                    //ValidAudience = JwtConfig.Instance.Audience,//Audience
                    // 动态验证，重新登录时，刷新token，旧token强制失效
                    AudienceValidator = (m, n, z) =>
                    {
                        var jwtToken = n as JwtSecurityToken;
                        if (jwtToken == null || m == null)
                        {
                            return false;
                        }
                        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value ?? "";
                        var userAudience = RedisCacheHelper.Get<string>($"{RedisKeys.UserAudience}_{userId}");
                        return userAudience != null && m.Contains(userAudience);
                    },

                    ValidateIssuer = true,//是否验证Issuer
                    ValidIssuer = JwtConfig.Instance.Issuer,//Issuer，这两项和前面签发jwt的设置一致

                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Instance.SecretKey))//拿到SecurityKey
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Query["access_token"];
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        // 将过期返回到请求头
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response?.Headers?.TryAdd("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // AppSettings 设置值
            AppSettings.Instance = Configuration.GetSection(nameof(AppSettings))?.Get<AppSettings>() ?? new AppSettings();

            // 内存缓存
            services.AddMemoryCache();

            // 公共
            services.AddScoped<ICommonService, CommonService>();
            // 后台
            services.AddScoped<ILoginService, LoginService>();
            // 官网
            services.AddScoped<IHomeService, HomeService>();


            //跨域设置
            services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            services.AddControllersWithViews()
                .AddMvcOptions(options => options.EnableEndpointRouting = false)
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                ; // 如果需要保持旧版路由行为
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // 以下两个配置应当写在 env.IsDevelopment() 判断中，调试模式启动
                // 为 Swagger 生成json文档和 Swagger UI 提供服务
                app.UseSwagger(c =>
                {
                    // Swashbuckle 会在 3.0 版规范（官方称为 OpenAPI 规范）中生成并公开 Swagger JSON。 为了支持向后兼容性，可以改为选择以 2.0 格式公开 JSON
                    //c.SerializeAsV2 = true;
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
                    // https://localhost:<port>/apiDoc 访问 swagger，默认为空 https://localhost:<port> 即可访问
                    // 自定义路径的话，还需要设置 Properties/launchSetting.json，注释 profiles下的IIS Express，appsettings.json 注释掉 urls
                    //c.RoutePrefix = "apiDoc";
                    c.RoutePrefix = string.Empty;
                    c.EnableDeepLinking();
                    c.ShowExtensions();
                    c.DisplayRequestDuration();
                    //c.DisplayOperationId();
                    c.DocExpansion(DocExpansion.None);
                    //c.EnableFilter();
                    c.EnableValidator();
                    c.ShowCommonExtensions();
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseResponseCompression();

            app.UseW3CLogging();

            app.UseHttpsRedirection();

            // 使用 wwwroot 静态文件
            app.UseStaticFiles();

            // 使用 resource 公共文件夹
            var resourcePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "resource");
            if (!Directory.Exists(resourcePath))
            {
                Directory.CreateDirectory(resourcePath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(resourcePath),
                RequestPath = "/resource"
            });

            app.UseRouting();

            // 添加jwt验证
            app.UseAuthentication();
            // 允许抛出 jwt 异常
            IdentityModelEventSource.ShowPII = true;

            // 使用 Auth
            app.UseAuthorization();

            MyHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            app.UseMvc(route =>
            {
                route.MapRoute(
                        name: "default",
                        template: "{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" }
                    );
                route.MapRoute(
                        name: "culture",
                        template: "{culture?}/{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" }
                    );
                route.MapRoute(
                        name: "api",
                        template: "api/{controller}/{action}/{id?}"
                    );
            });

        }
    }
}

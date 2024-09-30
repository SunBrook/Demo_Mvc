/*
 * @author: S 2024/9/29 19:26:52
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// Swagger配置
    /// </summary>
    public static class SwaggerSetup
    {
        /// <summary>
        /// 添加启动程序
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddSwaggerSetup(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            const string APINAME = "Demo_Mvc";
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"{APINAME} 接口文档",
                    //Description = $"{APINAME} HTTP API v1",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "联系人员",
                    //    Url = new Uri("https://example.com/contact")
                    //},
                    //License = new OpenApiLicense
                    //{
                    //    Name = "许可证",
                    //    Url = new Uri("https://example.com/license")
                    //}
                });

                c.OrderActionsBy(m => m.RelativePath);

                // 获取xml注释文件的目录
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "Demo_Mvc.xml");
                c.IncludeXmlComments(xmlPath, true);
                var commonPath = Path.Combine(AppContext.BaseDirectory, "Demo_Mvc.Common.xml");
                c.IncludeXmlComments(commonPath, true);

                //c.SchemaFilter<EnumSchemaFilter>();
                //c.DocumentFilter<SwaggerEnumFilter>();
            });

        }
    }
}

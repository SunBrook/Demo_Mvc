/*
 * @author: S 2024/9/29 19:36:10
 */

using Demo_Mvc.Common.Models.Request;
using Demo_Mvc.Common.Models;
using Demo_Mvc.Common.Tools;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo_Mvc.Service.BgManager.Login
{
    public class LoginService : ILoginService
    {
        private readonly MyDbContext _myDbContext;
        private readonly ILogger<LoginService> _logger;

        public LoginService(MyDbContext myDbContext, ILogger<LoginService> logger)
        {
            _myDbContext = myDbContext;
            _logger = logger;
        }

        public ApiResult<string> Login(RequestLogin requestModel)
        {
            try
            {
                _logger.LogInformation("登录, uid = {userName}, pwd = {password}", requestModel.UserName, requestModel.Password);

                if (string.IsNullOrEmpty(requestModel.UserName) || string.IsNullOrEmpty(requestModel.Password))
                {
                    return ApiResult<string>.Error("请输入用户名和密码");
                }

                using (_myDbContext)
                {
                    var userInfo = _myDbContext.Admins.FirstOrDefault(t => t.UserName == requestModel.UserName);
                    if (userInfo == null)
                    {
                        return ApiResult<string>.Error("用户名错误");
                    }

                    var passwordMD5 = ToolKit.GenPasswordMD5(requestModel.Password);
                    if (userInfo.Password != passwordMD5)
                    {
                        return ApiResult<string>.Error("密码错误");
                    }

                    if (userInfo.IsDeleted)
                    {
                        return ApiResult<string>.Error("用户已被删除，无法登录");
                    }

                    var now = DateTime.Now;

                    var issUser = JwtConfig.Instance.Issuer; // 颁发者
                    var secretKey = JwtConfig.Instance.SecretKey; // 密钥
                    var userAudienceKey = $"{RedisKeys.UserAudience}_{userInfo.Id}";
                    var audience = $"{new DateTimeOffset(now).ToUnixTimeSeconds()}"; // 每次登录更改
                    RedisCacheHelper.Set(userAudienceKey, audience, 86400);

                    var claims = new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Iss,issUser),
                        new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(now).ToUnixTimeSeconds()}"),
                        new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(now.AddHours(24)).ToUnixTimeSeconds()}"),
                        new Claim(ClaimTypes.PrimarySid, userInfo.Id.ToString()),
                        new Claim(ClaimTypes.Role, "Account"),
                        new Claim(ClaimTypes.Role, "System"),
                        new Claim(ClaimTypes.Name, requestModel.UserName),
                        new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(new LoginUser{
                            Id = userInfo.Id,
                            UserName = requestModel.UserName,
                            Roles = new string[]{ "Account", "System" },
                            Project = "Demo_MVC"
                        }))
                    };

                    // 对称密钥
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                    // 证书
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                    issuer: issUser,
                    audience: audience,
                    claims: claims,
                    expires: now.AddHours(24),
                    signingCredentials: creds);

                    // TODO 添加登录日志

                    return ApiResult.Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ApiResult<string>.Error(ex.Message);
            }
        }

        public ApiResult Logout()
        {
            try
            {
                _logger.LogInformation("登出, {userName}", Current.User.UserName);
                var result = RedisCacheHelper.Remove($"user_audience_{Current.User.Id}");

                // TODO 添加登出日志

                if (result)
                {
                    return ApiResult.Ok();
                }
                else
                {
                    return ApiResult.Error("退出登录失败，请稍后重试");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ApiResult.Error(ex.Message);
            }
        }
    }
}

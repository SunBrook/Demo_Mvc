/*
 * @author: S 2024/9/29 19:22:19
 */

using Demo_Mvc.Common.Models;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// 当前操作对象
    /// </summary>
    public class Current
    {
        /// <summary>
        /// 客户端ip地址
        /// </summary>
        public static string? ClientIp { get { return MyHttpContext.Context?.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString(); } }

        /// <summary>
        /// 登录用户
        /// </summary>
        public static LoginUser User
        {
            get
            {
                try
                {
                    if (MyHttpContext.Context?.User.Identity == null ||
                        MyHttpContext.Context?.User == null ||
                        !MyHttpContext.Context.User.Identity.IsAuthenticated)
                    {
                        return new LoginUser();
                    }

                    if (!(MyHttpContext.Context.User.Identity is ClaimsIdentity identity))
                    {
                        return new LoginUser();
                    }

                    if (int.TryParse(identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value, out int id) && id > 0)
                    {
                        string claimsValue = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value ?? string.Empty;
                        return JsonConvert.DeserializeObject<LoginUser>(claimsValue) ?? new LoginUser();
                    }
                }
                catch
                {
                }
                return new LoginUser();
            }
        }
    }
}

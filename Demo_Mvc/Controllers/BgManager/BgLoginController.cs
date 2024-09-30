
using Demo_Mvc.Common.Models;
using Demo_Mvc.Common.Models.Request;
using Demo_Mvc.Service.BgManager.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Mvc.Controllers.BgManager
{
    /// <summary>
    /// 登录模块
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BgLoginController
    {
        private readonly ILoginService _loginService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="loginService"></param>
        public BgLoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }


        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="requestModel">请求参数</param>
        /// <returns>返回Token</returns>
        [AllowAnonymous]
        [HttpPost]
        public ApiResult<string> Login([FromBody] RequestLogin requestModel)
        {
            return _loginService.Login(requestModel);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "accountSystem")]
        public ApiResult Logout()
        {
            return _loginService.Logout();
        }
    }
}

/*
 * @author: S 2024/9/29 19:35:38
 */

using Demo_Mvc.Common.Models.Request;
using Demo_Mvc.Common.Models;

namespace Demo_Mvc.Service.BgManager.Login
{
    public interface ILoginService
    {
        ApiResult<string> Login(RequestLogin requestModel);
        ApiResult Logout();
    }
}

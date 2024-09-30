/*
 * @author: S 2024/9/29 19:16:03
 */

using System.ComponentModel.DataAnnotations;

namespace Demo_Mvc.Common.Models.Request
{
    /// <summary>
    /// 请求：登录
    /// </summary>
    public class RequestLogin
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空"), StringLength(100)]
        public string UserName { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空"), StringLength(100)]
        public string Password { get; set; } = null!;
    }
}

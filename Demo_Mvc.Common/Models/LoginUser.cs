/*
 * @author: S 2024/9/29 19:18:20
 */

namespace Demo_Mvc.Common.Models
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class LoginUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public string[] Roles { get; set; } = null!;

        /// <summary>
        /// 所属项目
        /// </summary>
        public string Project { get; set; } = null!;
    }
}

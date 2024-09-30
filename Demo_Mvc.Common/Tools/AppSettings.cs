/*
 * @author: S 2024/9/29 19:22:01
 */

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// 项目配置类
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 单实例
        /// </summary>
        public static AppSettings Instance { get; set; } = null!;

        /// <summary>
        /// 密码盐
        /// </summary>
        public string PasswordSalt { get; set; } = null!;

        /// <summary>
        /// 域名地址
        /// </summary>
        public string Domain { get; set; } = null!;
    }
}

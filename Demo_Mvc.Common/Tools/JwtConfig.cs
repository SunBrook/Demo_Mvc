/*
 * @author: S 2024/9/29 19:25:01
 */

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// Jwt 配置
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// Jwt单例
        /// </summary>
        public static JwtConfig Instance { get; set; } = null!;
        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; } = null!;
        /// <summary>
        /// 哪些客户端可以使用
        /// </summary>
        public string Audience { get; set; } = null!;
        /// <summary>
        /// 密钥（长度必须大于等于16）
        /// </summary>
        public string SecretKey { get; set; } = null!;
    }
}

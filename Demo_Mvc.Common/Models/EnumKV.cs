/*
 * @author: S 2024/9/29 19:18:02
 */

namespace Demo_Mvc.Common.Models
{
    /// <summary>
    /// 枚举值和名称
    /// </summary>
    public class EnumKV
    {
        /// <summary>
        /// 枚举值
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 枚举名称
        /// </summary>
        public string Name { get; set; } = null!;
    }
}

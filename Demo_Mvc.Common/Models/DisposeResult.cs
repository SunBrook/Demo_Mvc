/*
 * @author: S 2024/9/29 19:17:40
 */

namespace Demo_Mvc.Common.Models
{
    /// <summary>
    /// 一般通用处理结果
    /// </summary>
    public class DisposeResult
    {
        /// <summary>
        /// True 成功， False 失败
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string? ErrorMsg { get; set; }
    }
}

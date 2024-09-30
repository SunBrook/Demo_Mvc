/*
 * @author: S 2024/9/29 19:20:04
 */

using Newtonsoft.Json;

namespace Demo_Mvc.Common.Models
{
    /// <summary>
    /// 查询分页模型
    /// </summary>
    public class QueryPageModel
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        [JsonProperty("pageIndex")]

        public int PageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        [JsonProperty("pageCount")]
        public int PageCount { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        [JsonProperty("totals")]
        public int Totals { get; set; }
    }
}

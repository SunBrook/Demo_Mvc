/*
 * @author: S 2024/9/29 19:37:26
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Mvc.Common.Models
{
    /// <summary>
    /// 常用的 RedisKey
    /// </summary>
    public class RedisKeys
    {
        /// <summary>
        /// JWT 允许用户使用的客户端
        /// </summary>
        public const string UserAudience = "user_audience";

        /// <summary>
        /// 首页数据
        /// </summary>
        public const string HomeModelData = "home_model_data";
    }
}

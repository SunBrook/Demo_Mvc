/*
 * @author: S 2024/9/29 19:22:41
 */

using Microsoft.AspNetCore.Http;

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// HttpContext
    /// </summary>
    public class MyHttpContext
    {
        private static IHttpContextAccessor _contextAccessor = null!;

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="contextAccessor"></param>
        public static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Context
        /// </summary>
        public static HttpContext? Context => _contextAccessor.HttpContext;
    }
}

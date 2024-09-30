/*
 * @author: S 2024/9/29 19:26:34
 */

using X.Web.Sitemap;
using X.Web.Sitemap.Extensions;

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// 站点地图工具类
    /// </summary>
    public class SiteMapHelper
    {
        /// <summary>
        /// 生成或更新站点地图
        /// </summary>
        /// <param name="urls">链接信息列表</param>
        /// <param name="savePath">存放路径</param>
        public static void Generate(List<Url> urls, string savePath)
        {
            var sitemap = new Sitemap(urls);
            sitemap.Save(savePath);
        }
    }
}

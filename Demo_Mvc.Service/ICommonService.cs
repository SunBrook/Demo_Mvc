/*
 * @author: S 2024/9/29 19:30:20
 */

using Demo_Mvc.Common.Models;

namespace Demo_Mvc.Service
{
    /// <summary>
    /// 公共调用
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// 更新站点地图
        /// </summary>
        /// <param name="dbContext"></param>
        void UpdateSiteMap(MyDbContext dbContext);

        /// <summary>
        /// 更新爬取规则
        /// </summary>
        /// <param name="dbContext"></param>
        void UpdateRobot(MyDbContext dbContext);
    }
}

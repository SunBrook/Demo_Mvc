/*
 * @author: S 2024/9/29 19:31:04
 */

using Demo_Mvc.Common.Models;
using Demo_Mvc.Common.Tools;
using Microsoft.Extensions.Logging;
using X.Web.Sitemap;

namespace Demo_Mvc.Service
{
    /// <summary>
    /// 公共业务
    /// </summary>
    public class CommonService : ICommonService
    {
        private readonly ILogger<CommonService> _logger;

        public CommonService(ILogger<CommonService> logger)
        {
            _logger = logger;
        }

        public void UpdateRobot(MyDbContext dbContext)
        {
            // UserAgent 列表
            var userAgents = new string[]
            {
                "Baiduspider",
                "Googlebot",
                "MSNBot",
                "Baiduspider-image",
                "YoudaoBot",
                "Sogou web spider",
                "Sogou inst spider",
                "Sogou spider2",
                "Sogou blog",
                "Sogou News Spider",
                "Sogou Orion spider",
                "ChinasoSpider",
                "Sosospider",
                "yisouspider",
                "EasouSpider"
            };

            // 禁止爬取
            var disAllows = new string[]
            {
                "/api/"
            };

            // 允许爬取
            var allows = new List<string>
            {
                "/Home"
            };

            List<string> robotList = new List<string>();
            foreach (var agent in userAgents)
            {
                robotList.Add($"User-agent: {agent}");

                // 禁止爬取
                foreach (var disItem in disAllows)
                {
                    robotList.Add($"Disallow: {disItem}");
                }

                // 允许爬取
                foreach (var allowItem in allows)
                {
                    robotList.Add($"Allow: {allowItem}");
                }

                // 空行
                robotList.Add(string.Empty);
            }

            robotList.Add("User-agent: *");
            robotList.Add("Disallow: /");

            // robot.txt 存放位置
            var savePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "robot.txt");
            // 更新
            FileKit.Write(robotList, savePath, true);
        }

        public void UpdateSiteMap(MyDbContext dbContext)
        {
            try
            {

                var now = DateTime.Now;
                var domain = AppSettings.Instance.Domain;

                var urls = new List<Url>
                {
                    // 首页
                    new Url
                    {
                        ChangeFrequency = ChangeFrequency.Weekly,
                        Location = $"{domain}/Home",
                        Priority = 0.9,
                        TimeStamp = now
                    }
                };


                // 存放路径
                var savePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "sitemap.xml");

                SiteMapHelper.Generate(urls, savePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}

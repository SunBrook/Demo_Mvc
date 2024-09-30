using Demo_Mvc.Service;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Demo_Mvc.Controllers
{
    public class BaseController : Controller
    {
        private readonly ICommonService _commonService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commonService"></param>
        public BaseController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// 判断并返回PC页面或者H5页面
        /// </summary>
        /// <param name="model"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public IActionResult Page(object? model = null, string? controller = null, string? action = null)
        {
            // 判断是否移动端或者PC端
            string userAgent = Request.Headers["User-Agent"].ToString().ToUpper();
            controller ??= RouteData.Values["controller"]?.ToString() ?? string.Empty;
            action ??= RouteData.Values["action"]?.ToString() ?? string.Empty;
            if (userAgent.Contains("MOBILE") || userAgent.Contains("PHONE") || userAgent.Contains("IPAD"))
            {
                // 返回H5页面
                var h5PageAddress = @$"~/Views/{controller}/{action}_h5.cshtml";
                return View(h5PageAddress, model);
            }
            else
            {
                // 返回pc页面
                var pcPageAddress = @$"~/Views/{controller}/{action}.cshtml";
                return View(pcPageAddress, model);
            }
        }

        /// <summary>
        /// 页面基础配置: 语种、布局页、标题设置，返回语种Id
        /// </summary>
        public int InitGetLangId()
        {
            // 域名
            ViewData["url"] = $@"{Request.Scheme}://{Request.Host}";

            // 语种
            var langIdentify = GetLangIdentify();

            // 标题
            var controller = RouteData.Values["controller"]?.ToString() ?? string.Empty;
            switch (controller)
            {
                case "Home":    // 首页
                    ViewBag.Title = "首页";
                    break;
            }

            return 0;
        }

        /// <summary>
        /// 获取语种标识
        /// </summary>
        /// <returns>语种标识</returns>
        public string GetLangIdentify()
        {
            // 多语种
            string controller = RouteData.Values["controller"]?.ToString() ?? string.Empty;
            string culture = RouteData.Values["culture"]?.ToString() ?? string.Empty;

            string lang;
            if (culture == string.Empty)
            {
                // 地址栏没有语种标识，从浏览器取语种标识
                lang = Request.Headers["Accept-Language"].ToString();
            }
            else
            {
                lang = culture;
            }

            var cultureInfo = CultureInfo.CreateSpecificCulture(lang);
            return cultureInfo.Name;
        }
    }
}

using Demo_Mvc.Common.Models.ViewModel;
using Demo_Mvc.Service;
using Demo_Mvc.Service.OfficialWebSite.Home;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Mvc.Controllers.OfficialWebSite
{
    public class HomeController : BaseController
    {
        private readonly IHomeService _homeService;
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commonService"></param>
        /// <param name="homeService"></param>
        /// <param name="logger"></param>
        public HomeController(ICommonService commonService, IHomeService homeService, ILogger<HomeController> logger) : base(commonService)
        {
            _homeService = homeService;
            _logger = logger;
        }

        /// <summary>
        /// Home
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            try
            {
                // 初始化，并获取语种Id
                int langId = InitGetLangId();
                HomeModel model = _homeService.GetIndexModel(langId);
                return Page(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return NotFound();
            }
        }
    }
}

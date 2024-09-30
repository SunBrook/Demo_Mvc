/*
 * @author: S 2024/9/29 19:43:05
 */

using Demo_Mvc.Common.Models;
using Demo_Mvc.Common.Models.ViewModel;
using Demo_Mvc.Common.Tools;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Demo_Mvc.Service.OfficialWebSite.Home
{

    public class HomeService : IHomeService
    {
        private readonly MyDbContext _myDbContext;
        private readonly ILogger<HomeService> _logger;
        private readonly ICommonService _commonService;
        private readonly IMemoryCache _memoryCache;

        public HomeService(MyDbContext myDbContext, ILogger<HomeService> logger, ICommonService commonService, IMemoryCache memoryCache)
        {
            _myDbContext = myDbContext;
            _logger = logger;
            _commonService = commonService;
            _memoryCache = memoryCache;
        }

        public HomeModel GetIndexModel(int langId)
        {
            try
            {
                // 尝试从缓存读取数据，如果没有，则从数据库中读取
                if (_memoryCache.TryGetValue($"{MemoryCacheKeys.HomeModelData}_{langId}", out HomeModel? memoryHomeModel))
                {
                    if (memoryHomeModel != null)
                    {
                        return memoryHomeModel;
                    }
                }

                var redisHomeModel = RedisCacheHelper.Get<HomeModel>($"{RedisKeys.HomeModelData}_{langId}");
                if (redisHomeModel != null)
                {
                    return redisHomeModel;
                }

                using (_myDbContext)
                {
                    var model = _myDbContext.Admins.FirstOrDefault();
                    if (model != null)
                    {
                        HomeModel result = new HomeModel { Id = model.Id, Name = model.UserName };
                        return result;
                    }
                    return new HomeModel { };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new HomeModel();
            }
        }
    }
}

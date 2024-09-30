/*
 * @author: S 2024/9/29 19:41:00
 */

using Demo_Mvc.Common.Models.ViewModel;

namespace Demo_Mvc.Service.OfficialWebSite.Home
{
    public interface IHomeService
    {
        HomeModel GetIndexModel(int langId);
    }
}

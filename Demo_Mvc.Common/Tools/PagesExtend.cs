/*
 * @author: S 2024/9/29 19:25:22
 */

using Demo_Mvc.Common.Models;

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// 分页扩展
    /// </summary>
    public static class PagesExtend
    {

        /// <summary>
        /// 获得分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Queryable">LINQ表达式</param>
        /// <param name="PageID">页码，为空时表示1</param>
        /// <param name="PageSize">每页条数，默认20页</param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static List<T> getPages<T>(this IQueryable<T> Queryable, int PageID, int PageSize, QueryPageModel t2)
        {
            var result = getPages<T>(Queryable, PageID, PageSize, out int Pages, out int recordCount);
            t2.PageIndex = PageID;
            t2.PageSize = PageSize;
            t2.PageCount = Pages;
            t2.Totals = recordCount;
            return result;
        }

        /// <summary>
        /// 获得分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Queryable"></param>
        /// <param name="PageID"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static ApiResult<List<T>> GetPages<T>(this IQueryable<T> Queryable, int PageID, int PageSize)
        {
            var result = getPages<T>(Queryable, PageID, PageSize, out int Pages, out int recordCount);
            var apiResult = ApiResult.Ok(result);
            apiResult.PageIndex = PageID;
            apiResult.PageSize = PageSize;
            apiResult.PageCount = Pages;
            apiResult.Totals = recordCount;
            return apiResult;
        }


        /// <summary>
        /// 获得分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Queryable">LINQ表达式</param>
        /// <param name="PageID">页码，为空时表示1</param>
        /// <param name="PageSize">每页条数，默认20页</param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static List<T> getPages<T>(this List<T> Queryable, int PageID, int PageSize, QueryPageModel t2)
        {
            var result = getPages<T>(Queryable, PageID.ToString(), PageSize.ToString(), out int Pages, out int recordCount);
            t2.PageIndex = PageID;
            t2.PageSize = PageSize;
            t2.PageCount = Pages;
            t2.Totals = recordCount;
            return result;
        }


        /// <summary>
        /// 获得分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Queryable">LINQ表达式</param>
        /// <param name="PageID">页码，为空时表示1</param>
        /// <param name="PageSize">每页条数，默认20页</param>
        /// <returns></returns>
        public static List<T> getPages<T>(this IQueryable<T> Queryable, int PageID, int PageSize)
        {
            return getPages<T>(Queryable, PageID, PageSize, out int Pages, out int recordCount);
        }

        /// <summary>
        /// 获得分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Queryable">LINQ表达式</param>
        /// <param name="PageID">页码，为空时表示1</param>
        /// <param name="PageSize">每页条数，默认20页</param>
        /// <param name="Pages">总页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public static List<T> getPages<T>(this IQueryable<T> Queryable, string PageID, string PageSize, out int Pages, out int recordCount)
        {
            recordCount = Queryable.Count();
            //是否合法的页码
            if (string.IsNullOrEmpty(PageID) || !PageID.IsInt()) { PageID = "1"; }
            if (PageID.ToInt() == 0) { PageID = "1"; }
            //页号是否未传
            if (string.IsNullOrEmpty(PageSize) || !PageSize.IsInt()) { PageSize = "20"; }
            Pages = (recordCount + PageSize.ToInt() - 1) / PageSize.ToInt();
            if (Pages == 0) { Pages = 1; }
            //判断指定的页码是否大于最大页码
            //if (PageID.ToInt() > Pages) { PageID = Pages.ToString(); }
            if (PageID.ToInt() > Pages)
            {
                return Queryable.Take(0).ToList() ?? new List<T>();
            }
            else
            {
                //写记录数、页数
                return Queryable.Skip((PageID.ToInt() - 1) * PageSize.ToInt()).Take(PageSize.ToInt()).ToList<T>() ?? new List<T>();
            }
        }

        /// <summary>
        /// 获得分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Queryable">LINQ表达式</param>
        /// <param name="PageID">页码，为空时表示1</param>
        /// <param name="PageSize">每页条数，默认20页</param>
        /// <param name="Pages">总页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public static List<T> getPages<T>(this IQueryable<T> Queryable, int PageID, int PageSize, out int Pages, out int recordCount)
        {
            return getPages(Queryable, PageID.ToString(), PageSize.ToString(), out Pages, out recordCount);
        }


        /// <summary>
        /// 获得分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Queryable">LINQ表达式</param>
        /// <param name="PageID">页码，为空时表示1</param>
        /// <param name="PageSize">每页条数，默认20页</param>
        /// <param name="Pages">总页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public static List<T> getPages<T>(this List<T> Queryable, string PageID, string PageSize, out int Pages, out int recordCount)
        {
            recordCount = Queryable.Count();
            //是否合法的页码
            if (string.IsNullOrEmpty(PageID) || !PageID.IsInt()) { PageID = "1"; }
            if (PageID.ToInt() == 0) { PageID = "1"; }
            //页号是否未传
            if (string.IsNullOrEmpty(PageSize) || !PageSize.IsInt()) { PageSize = "20"; }
            Pages = (recordCount + PageSize.ToInt() - 1) / PageSize.ToInt();
            if (Pages == 0) { Pages = 1; }
            //判断指定的页码是否大于最大页码
            //if (PageID.ToInt() > Pages) { PageID = Pages.ToString(); }
            if (PageID.ToInt() > Pages)
            {
                return Queryable.Take(0).ToList();
            }
            else
            {
                //写记录数、页数
                return Queryable.Skip((PageID.ToInt() - 1) * PageSize.ToInt()).Take(PageSize.ToInt()).ToList<T>();
            }
        }
    }
}

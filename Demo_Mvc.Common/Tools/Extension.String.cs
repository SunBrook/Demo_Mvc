/*
 * @author: S 2024/9/29 19:23:47
 */

using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// 拓展方法
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// 根据 html 标签分隔列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string>? SplitHtmlList(this string str)
        {
            if (str == null) return new List<string>();
            // 将正常换行符提前替换
            string tempStr = "[_TMP_]";
            var replaceBR = Regex.Replace(str, "<[b|B][rR].*?>", tempStr);
            var replaceP = Regex.Replace(replaceBR, "<[p|P]*/>", tempStr);
            var replaceHtml = Regex.Replace(replaceP, "<.*?>", "<br/>");
            var htmlList = replaceHtml.Split("<br/>")
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Select(t => t == tempStr ? string.Empty : t)
                    .ToList();
            return htmlList;
        }

        /// <summary>
        /// 清理 html 标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearHtml2(this string str)
        {
            var replaceHtml = Regex.Replace(str, "<.*?>", string.Empty);
            return replaceHtml;
        }

        /// <summary>
        /// 生成文本和<br/>拼接的Html内容
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string? StringJoinBrHtml(this List<string>? list)
        {
            if (list == null || list.Count == 0) return string.Empty;
            return string.Join("<br/>", list);
        }

        /// <summary>
        /// 分隔逗号的Id，生成列表
        /// </summary>
        /// <param name="str">逗号分隔的Id字符串</param>
        /// <returns>id列表</returns>
        public static List<int> SplitIds(this string str)
        {
            if (string.IsNullOrEmpty(str)) return new List<int>();
            return str.Split(",").Select(int.Parse).ToList();
        }

        /// <summary>
        /// 返回字符串反序列化列表，如果为空则返回空列表
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="str">JSON字符串</param>
        /// <returns>列表</returns>
        public static List<T> JsonDesList<T>(this string str)
        {
            return !string.IsNullOrEmpty(str)
                            ? JsonConvert.DeserializeObject<List<T>>(str) ?? new List<T>()
                            : new List<T>();
        }


    }
}

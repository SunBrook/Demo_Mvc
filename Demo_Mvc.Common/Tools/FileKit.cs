/*
 * @author: S 2024/9/29 19:24:19
 */

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public class FileKit
    {
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="strList">多行文本列表</param>
        /// <param name="savePath">存储路径</param>
        /// <param name="isCover">是否覆盖源文件内容</param>
        public static void Write(List<string> strList, string savePath, bool isCover = false)
        {
            using (StreamWriter sw = new StreamWriter(savePath, !isCover))
            {
                foreach (var str in strList)
                {
                    sw.WriteLine(str);
                }
            }
        }
    }
}

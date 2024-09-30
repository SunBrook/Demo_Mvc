/*
 * @author: S 2024/9/29 19:27:11
 */

using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static partial class ToolKit
    {
        /// <summary>
        /// 将一个字符串转换为Int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this object str)
        {
            return Convert.ToInt32(str);
        }

        /// <summary>
        /// 转换为decimal
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public static decimal ToDecimal<T>(this T str, int round)
        {
            return Math.Round(Convert.ToDecimal(str), round, MidpointRounding.AwayFromZero);
        }

        /// <summary> 
        /// 过滤源代码中的HTML 
        /// </summary> 
        /// <param name="html"></param> 
        /// <returns></returns> 
        /// <remarks></remarks> 
        public static string ClearHtml(this string? html)
        {
            if (html == null) return "";
            if (html.Trim() == "") { return ""; }
            Regex Regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            string strOutput = Regex.Replace(html, "");
            return strOutput;
        }

        /// <summary> 
        /// 转换文本中的Html开始及闭合标签 
        /// </summary> 
        /// <param name="html"></param> 
        /// <returns></returns> 
        /// <remarks></remarks> 
        public static string ConvertHtml(this string html)
        {
            html = html.Replace(">", "&lt;");
            html = html.Replace("<", "&gt;");
            return html;
        }

        /// <summary>
        /// 是否整数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Boolean IsInt(this string number)
        {
            int Num = 0;
            return int.TryParse(number, out Num);
        }

        /// <summary>
        /// 生成密码加密字符
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static string GenPasswordMD5(string password)
        {
            var salt = AppSettings.Instance.PasswordSalt;
            string str = $"{salt}{password}{salt}";
            using (var md5 = MD5.Create())
            {
                // 将输入字符串转换为字节数组
                byte[] inputBytes = Encoding.UTF8.GetBytes(str);
                // 计算散列
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                // 将字节转换为十六进制字符串
                StringBuilder hashString = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashString.Append(hashBytes[i].ToString("X2"));
                }
                return hashString.ToString();
            }
        }
    }
}

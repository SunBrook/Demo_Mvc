/*
 * @author: S 2024/9/29 19:23:27
 */

using Demo_Mvc.Common.Models;

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// Enum工具类
    /// </summary>
    public class EnumKit
    {
        /// <summary> 
        /// 根据枚举项名称或得对应的枚举值 
        /// </summary> 
        /// <param name="enumType"></param> 
        /// <param name="name"></param> 
        /// <returns></returns> 
        /// <remarks></remarks> 
        public static int GetValue(Type enumType, string name)
        {
            return Convert.ToInt32(Enum.Format(enumType, Enum.Parse(enumType, name), "d"));
        }

        /// <summary>
        /// 获取名称和对应的值的列表 List
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static List<EnumKV> GetEnumKVs(Type enumType)
        {
            var list = new List<EnumKV>();
            var vals = Enum.GetValues(enumType);

            foreach (int val in vals)
            {
                list.Add(new EnumKV
                {
                    Id = val,
                    Name = GetName(enumType, val) ?? string.Empty
                });
            }

            return list;
        }

        /// <summary> 
        /// 获得指定值的枚举项名称 
        /// </summary> 
        /// <param name="enumType"></param> 
        /// <param name="Value"></param> 
        /// <returns></returns> 
        /// <remarks></remarks> 
        public static string? GetName(Type enumType, int Value)
        {
            return Enum.GetName(enumType, Value);
        }

    }
}

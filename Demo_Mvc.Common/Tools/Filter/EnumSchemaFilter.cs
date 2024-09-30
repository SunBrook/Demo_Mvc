/*
 * @author: S 2024/9/29 19:21:12
 */

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Demo_Mvc.Common.Tools.Filter
{
    /// <summary>
    /// swagger文档生成过滤器，用于枚举描述的生成
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// 应用
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(name =>
                    {
                        Enum e = (Enum)Enum.Parse(context.Type, name);
                        model.Enum.Add(new OpenApiString($"{name}={Convert.ToInt64(Enum.Parse(context.Type, name))}"));
                    });

            }
        }
    }
}

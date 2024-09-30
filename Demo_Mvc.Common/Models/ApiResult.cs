/*
 * @author: S 2024/9/29 19:17:12
 */

using Newtonsoft.Json;

namespace Demo_Mvc.Common.Models
{
    /// <summary>
    /// 返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// 数据主体
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; } = default!;

        /// <summary>
        /// 返回异常
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public new static ApiResult<T> Error(string message)
        {
            return new ApiResult<T>
            {
                Success = false,
                Message = message,
                Code = 500
            };
        }

        /// <summary>
        /// 404 not found
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public new static ApiResult<T> NotFound(string? message = null)
        {
            return new ApiResult<T>
            {
                Code = 404,
                Success = false,
                Message = message ?? "not found"
            };
        }

        /// <summary>
        /// 没有内容
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public new static ApiResult<T> NoContent(string? message = null)
        {
            return new ApiResult<T>
            {
                Code = 204,
                Message = message ?? "no content",
                Success = true,
            };
        }
    }

    /// <summary>
    /// 接口返回类
    /// </summary>
    public class ApiResult : QueryPageModel
    {
        /// <summary>
        /// 状态值
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// 请求是否成功
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; } = null!;

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public static ApiResult Ok()
        {
            return new ApiResult
            {
                Code = 200,
                Success = true
            };
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <typeparam name="T">返回Data类型</typeparam>
        /// <param name="data">data数据</param>
        /// <returns></returns>
        public static ApiResult<T> Ok<T>(T data)
        {
            return new ApiResult<T>
            {
                Code = 200,
                Success = true,
                Data = data
            };
        }

        /// <summary>
        /// 没有内容
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult NoContent(string? message = null)
        {
            return new ApiResult
            {
                Code = 204,
                Message = message ?? "no content",
                Success = true
            };
        }

        /// <summary>
        /// 404 not found
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult NotFound(string? message = null)
        {
            return new ApiResult
            {
                Code = 404,
                Success = false,
                Message = message ?? "not found"
            };
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult Error(string message)
        {
            return new ApiResult
            {
                Code = 500,
                Success = false,
                Message = message
            };
        }
    }
}

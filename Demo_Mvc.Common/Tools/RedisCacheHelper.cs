/*
 * @author: S 2024/9/29 19:26:10
 */

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System.Text;
using System.Text.Json;

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// Redis 工具类
    /// </summary>
    public class RedisCacheHelper
    {
        private static RedisCache? _redisCache = null;
        private static RedisCacheOptions? options = null;

        /// <summary>
        /// 实例名称
        /// </summary>
        public static string? InstanceName = null;

        /// <summary>
        /// 初始化 Redis
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="instanceName"></param>
        public RedisCacheHelper(string connectionString, string instanceName)
        {
            InstanceName = instanceName;
            options = new RedisCacheOptions
            {
                Configuration = connectionString,
                InstanceName = instanceName,
            };
            _redisCache = new RedisCache(options);
        }

        /// <summary>
        /// 设置redis 值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">数据字符串值</param>
        /// <param name="exprieTime">过期时间</param>
        /// <returns></returns>
        public static bool SetStringValue(string key, string value, int? exprieTime = null)
        {
            try
            {
                if (_redisCache == null)
                {
                    throw new Exception($"redisCache is null");
                }

                if (exprieTime.HasValue)
                {
                    _redisCache.SetString(key, value, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(exprieTime.Value)
                    });
                }
                else
                {
                    _redisCache.SetString(key, value);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取 redis 数据，字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetStringValue(string key)
        {
            try
            {
                if (_redisCache == null)
                {
                    throw new Exception($"redisCache is null");
                }

                return _redisCache.GetString(key) ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static T? Get<T>(string key)
        {
            try
            {
                string value = GetStringValue(key);
                if (string.IsNullOrEmpty(value))
                {
                    return default;
                }
                else
                {
                    var obj = JsonSerializer.Deserialize<T>(value);
                    return obj;
                }
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="exprieTime">过期时间</param>
        /// <returns></returns>
        public static bool Set<T>(string key, T value, int? exprieTime = null)
        {
            try
            {
                var valueStr = JsonSerializer.Serialize(value);
                return SetStringValue(key, valueStr, exprieTime);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            try
            {
                if (_redisCache == null)
                {
                    throw new Exception($"redisCache is null");
                }
                _redisCache.Remove(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <returns></returns>
        public static bool Refresh(string key)
        {
            try
            {
                if (_redisCache == null)
                {
                    throw new Exception($"redisCache is null");
                }
                _redisCache.Refresh(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="exprieTime"></param>
        /// <returns></returns>
        public static bool Replace(string key, string value, int? exprieTime = null)
        {
            try
            {
                return SetStringValue(key, value, exprieTime);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="exprieTime"></param>
        /// <returns></returns>
        public static bool Replace<T>(string key, T value, int? exprieTime = null)
        {
            try
            {
                var valueStr = JsonSerializer.Serialize(value);
                return SetStringValue(key, valueStr, exprieTime);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

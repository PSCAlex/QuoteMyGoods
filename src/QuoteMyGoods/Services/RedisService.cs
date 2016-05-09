﻿using QMGAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Services
{
    public interface IRedisService
    {
        void SetString(string key, string value);
        Task<string> GetString(string key);
        void SetObject(string key, object value);
        object GetObject<T>(string key);
    }
    public class RedisService : IRedisService
    {
        private RedisCache _redisCache;

        public RedisService()
        {
            _redisCache = new RedisCache();
        }

        public object GetObject<T>(string key)
        {
            return _redisCache.GetObject<T>(key);
        }

        public async Task<string> GetString(string key)
        {
            return await _redisCache.GetString(key);
        }

        public void SetObject(string key, object value)
        {
            _redisCache.SetObject(key, value);
        }

        public void SetString(string key, string value)
        {
            _redisCache.SetString(key, value);
        }
    }
}
using Microsoft.Extensions.Caching.Distributed;
using ShiftSwift.Application.services.caching;
using System.Text.Json;

namespace ShiftSwift.Infrastructure.services.caching;

//public class RedisCacheService : ICacheService
//{
//    private readonly IDistributedCache _cache;

//    public RedisCacheService(IDistributedCache cache)
//    {
//        _cache = cache;
//    }

//    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
//    {
//        var json = JsonSerializer.Serialize(value);
//        await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
//        {
//            AbsoluteExpirationRelativeToNow = expiration
//        });
//    }

//    public async Task<T> GetAsync<T>(string key)
//    {
//        var json = await _cache.GetStringAsync(key);
//        return json == null ? default : JsonSerializer.Deserialize<T>(json);
//    }
//}
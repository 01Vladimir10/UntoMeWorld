using System.Collections.Concurrent;
using UntoMeWorld.WasmClient.Server.Common.Model;
using UntoMeWorld.WasmClient.Server.Services;
using UntoMeWorld.WasmClient.Server.Services.Base;
using Timer = System.Timers.Timer;

namespace UntoMeWorld.WasmClient.Server.Common;

public class InMemoryCache : ICacheService
{
    private readonly IDictionary<string, CacheEntry<object>> _cachedValues;
    private Timer _timer;

    public InMemoryCache()
    {
        _cachedValues = new ConcurrentDictionary<string, CacheEntry<object>>();
        InitGarbageCollector();
    }

    private void InitGarbageCollector()
    {
        _timer = new Timer(1000 * 60 * 5);
        _timer.Enabled = true;
        _timer.AutoReset = true;
        _timer.Elapsed += (_, _) =>
        {
            if (!_cachedValues.Any())
                return;
            var expiredKeys = _cachedValues.Where(e => e.Value.IsExpired).Select(e => e.Key);
            foreach (var expiredKey in expiredKeys)
                _cachedValues.Remove(expiredKey);
        };
    }

    public Task<T> GetEntry<T>(string key, Func<Task<T>> callback)
        => GetEntry(key, async () => new CacheEntry<T>
        {
            Data = await callback()
        });
    public async Task<T> GetEntry<T>(string key, Func<Task<CacheEntry<T>>> callback)
    {
        var entry = _cachedValues.ContainsKey(key) ? _cachedValues[key] : null;
        if (entry is {IsExpired: false})
            return entry.Data is T data ? data : default;
        
        var result = await callback();
        
        if (result.LifeSpan == TimeSpan.Zero)
            return result.Data;
        
        _cachedValues[key] = new CacheEntry<object>
        {
            Data = result.Data,
            LifeSpan = result.LifeSpan
        };
        return result.Data;
    }

    public void SetEntry(string key, object value)
    {
        _cachedValues[key] = new CacheEntry<object>{Data = value};
    }

    public void SetEntry<T>(string key, CacheEntry<T> entry)
    {
        _cachedValues[key] = new CacheEntry<object>
        {
            LifeSpan = entry.LifeSpan,
            Data = entry.Data
        };
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _cachedValues.Clear();
        _timer?.Dispose();
    }
}
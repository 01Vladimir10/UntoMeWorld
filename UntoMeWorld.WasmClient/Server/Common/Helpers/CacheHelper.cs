using Microsoft.Extensions.Caching.Memory;

namespace UntoMeWorld.WasmClient.Server.Common.Helpers;

public class CacheHelper<T, TKey> : IDisposable
{
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _lifeSpan;
    private readonly string _prefix;
    private readonly IDictionary<string, Func<T, object>> _indicesSelectors;

    public CacheHelper(IMemoryCache memoryCache, string prefix = null, TimeSpan lifeSpan = default)
    {
        _memoryCache = memoryCache;
        _lifeSpan = lifeSpan;
        _prefix = prefix ?? $"{typeof(T).Name}__";
        _indicesSelectors = new Dictionary<string, Func<T, object>>();
    }

    private string BuildKey(TKey key) => _prefix + key;

    private string BuildIndexKey(string propertyName, object propertyKey) =>
        _prefix + "__index__" + propertyName + "." + propertyKey;

    public Task<T> Get(TKey key, Func<Task<T>> callback)
        => _memoryCache.GetOrCreateAsync(BuildKey(key), async entry =>
        {
            entry.SetSlidingExpiration(_lifeSpan);
            entry.Size = 1;
            var value = await callback();
            UpdateIndices(key, value);
            return value;
        });

    public void Set(TKey key, T item)
    {
        _memoryCache.Set(BuildKey(key), item, new MemoryCacheEntryOptions {Size = 1});
        UpdateIndices(key, item);
    }
    public void Set(Func<T, TKey> keySelector, params T[] items)
    {
        foreach (var item in items)
            Set(keySelector(item), item);
    }
    private void UpdateIndices(TKey key, T item)
    {
        foreach (var (propertyName, propertySelector) in _indicesSelectors)
        {
            var propertyValue = propertySelector(item);
            _memoryCache.Set(BuildIndexKey(propertyName, propertyValue), key, new MemoryCacheEntryOptions {Size = 1});
        }
    }

    public void CreateIndex(string propertyName, Func<T, object> propertySelector)
    {
        _indicesSelectors[propertyName] = propertySelector;
    }

    public T GetOrDefault(TKey key)
        => _memoryCache.TryGetValue<T>(BuildKey(key), out var value) ? value : default;

    public async Task<T> GetByIndexedProperty(string indexPropertyName, object value, Func<Task<(TKey, T)>> callback)
    {
        if (!_indicesSelectors.ContainsKey(indexPropertyName))
            return default;
        if (_memoryCache.TryGetValue<TKey>(BuildIndexKey(indexPropertyName, value), out var itemKey))
        {
            return await Get(itemKey, async () =>
            {
                var (_, i) = await callback();
                return i;
            });
        }

        var (key, item) = await callback();
        Set(key, item);
        return item;
    }
    public T GetByIndexedPropertyOrDefault(string indexPropertyName, object value)
    {
        if (!_indicesSelectors.ContainsKey(indexPropertyName))
            return default;
        return _memoryCache.TryGetValue<TKey>(BuildIndexKey(indexPropertyName, value), out var itemKey) ? GetOrDefault(itemKey) : default;
    }

    public void InvalidateItem(params TKey[] keys)
    {
        foreach (var key in keys)
            if (_memoryCache.TryGetValue(BuildKey(key), out _))
                Set(key, default);
    }
    public void UpdateIfPresent(TKey key, Func<T, T> updateFunction)
    {
        var entry = GetOrDefault(key);
        if (entry == null)
            return;
        Set(key,  updateFunction(entry));
    } 
    public void UpdateIfPresent(Func<T, TKey, T> updateFunction, params TKey[] keys)
    {
        foreach (var key in keys)
        {
            var entry = GetOrDefault(key);
            if (entry == null)
                continue;
            Set(key,updateFunction(entry, key));
        }
    }

    public void Dispose()
    {
        _indicesSelectors.Clear();
        _memoryCache?.Dispose();
    }
}
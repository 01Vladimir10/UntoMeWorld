using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Common.Model;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Services.Abstractions;

public abstract class GenericSecurityService<T> : ISecurityService<T> where T : IModel
{
    private readonly ISecurityStore<T> _store;
    protected readonly string CachePrefix;
    protected readonly TimeSpan CacheLifespan;
    protected readonly bool EnableCache;
    protected readonly ICacheService Cache;

    protected GenericSecurityService(ISecurityStore<T> store, ICacheService cache, bool enableCache, string cachePrefix,
        TimeSpan cacheLifespan)
    {
        _store = store;
        CachePrefix = cachePrefix;
        CacheLifespan = cacheLifespan;
        Cache = cache;
        EnableCache = enableCache;
    }

    public async Task<T> Add(T item)
    {
        var createdItem = await _store.Add(item);
        if (EnableCache) 
            Cache.SetEntry(createdItem.Id, BuildCacheEntry(createdItem));
        return createdItem;
    }

    public Task<T> Get(string id) => ExecuteDatabaseQuery(id, () => _store.Get(id));
    public Task<List<T>> GetAll() => _store.All();

    public async Task<T> Update(T item)
        => (await UpdateItems(item)).FirstOrDefault();
    public Task<IEnumerable<T>> Update(IEnumerable<T> items)
        => UpdateItems(items.ToArray());

    protected CacheEntry<T> BuildCacheEntry(T item)
        => new()
        {
            Data = item,
            LifeSpan = CacheLifespan
        };
    private async Task<IEnumerable<T>> UpdateItems(params T[] items)
    {
        await _store.Update(items);
        if (!EnableCache) return items;
        
        foreach (var item in items)
            Cache.SetEntry(CachePrefix + item.Id, new CacheEntry<T>
            {
                LifeSpan = CacheLifespan,
                Data = item
            });
        return items;
    }
    private Task<T> ExecuteDatabaseQuery(string key, Func<Task<T>> func)
        => EnableCache
            ? Cache.GetEntry(CachePrefix + key, async () => BuildCacheEntry(await func()))
            : func();

    public async Task Delete(params string[] ids)
    {
        if (EnableCache)
        {
            foreach (var id in ids)
            {
                var item = await Cache.Get<Token>(CachePrefix + id);
                if (item == null)
                    continue;
                // Invalidate the cache entries.
                // If we were to remove them from the cache, there would be
                // innecessary calls to the database when receiving an invalid token.
                Cache.SetEntry(CachePrefix + id, BuildCacheEntry(default));
            }
        }
        await _store.Delete(ids);
    }
}  
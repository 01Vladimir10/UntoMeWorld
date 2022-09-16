using UntoMeWorld.Application.Helpers;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.WasmClient.Server.Services.Abstractions;

public abstract class GenericSecurityService<T> : ISecurityService<T> where T : IModel
{
    private readonly ISecurityStore<T> _store;
    protected readonly CacheHelper<T, string> Cache;
    protected readonly bool EnableCache;

    protected GenericSecurityService(ISecurityStore<T> store, CacheHelper<T, string> cache, bool enableCache = true)
    {
        _store = store;
        Cache = cache;
        EnableCache = enableCache;
    }

    public async Task<T> Add(T item)
    {
        var createdItem = await _store.Add(item);
        if (EnableCache)
            Cache.Set(createdItem.Id, createdItem);
        return createdItem;
    }

    public Task<T> Get(string id) => EnableCache ? Cache.Get(id, () => _store.Get(id)) : _store.Get(id);
    public Task<List<T>> GetAll() => _store.All();

    public async Task<T> Update(T item)
    {
        await _store.Update(item);
        if (EnableCache)
            Cache.Set(item.Id, item);
        return item;
    }

    public async Task<IEnumerable<T>> Update(IEnumerable<T> items)
    {
        var itemsArr = items as T[] ?? Array.Empty<T>();
        await _store.Update(itemsArr);
        if (!EnableCache) return items;
        
        foreach (var item in itemsArr)
            Cache.Set(item.Id, item);
        return items;
    }


    public async Task Delete(params string[] ids)
    {
        await _store.Delete(ids);
        if (EnableCache)
            foreach (var id in ids)
                Cache.InvalidateItem(id);
    }
}  
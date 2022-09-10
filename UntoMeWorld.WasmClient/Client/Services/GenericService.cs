using UntoMeWorld.Application.Common;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.WasmClient.Client.Services.Base;
using UntoMeWorld.WasmClient.Client.Utils.Common;
using static UntoMeWorld.Domain.Common.QueryLanguage;

namespace UntoMeWorld.WasmClient.Client.Services;
/// <summary>
/// Provides a simple abstraction of the stores and implements caching.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class GenericService<T> : IService<T> where T : IModel, IRecyclableModel
{
    private readonly IStore<T> _store;
    private readonly SimpleTaskManager<PaginationResult<T>> _taskManager = new();

    protected GenericService(IStore<T> store)
    {
        _store = store;
    }

    public async Task<PaginationResult<T>> Paginate(QueryFilter filter = null, string textQuery = null, string orderBy = null, bool orderDesc = false,
        int page = 1, int pageSize = 30)
    {
        // Ensure that only non deleted items will be returned.
        var finalQuery = filter == null
            ? Eq(nameof(IRecyclableModel.IsDeleted), false)
            : And(Eq(nameof(IRecyclableModel.IsDeleted), false), filter);
        
        return await _taskManager.ExecuteTask(async () =>
            {
                await Task.Yield();
                return await _store.Query(finalQuery, textQuery ?? string.Empty, orderBy ?? string.Empty, orderDesc, page, pageSize);
            });
    }

    public async Task<List<T>> All()
    {
        var result = await Paginate(null, pageSize: 100000, page:1);
        return result.Result;
    }

    public Task<PaginationResult<T>> PaginateDeleted(QueryFilter filter = null, 
        string textQuery = null,
        string orderBy = null,
        bool orderDesc = false, int page = 1,
        int pageSize = 30)
    {
        // Ensure that only non deleted items will be returned.
        var finalQuery = filter == null
            ? Eq(nameof(IRecyclableModel.IsDeleted), true)
            : And(Eq(nameof(IRecyclableModel.IsDeleted), false), filter);
        
        return _store.Query(finalQuery, textQuery ?? string.Empty, orderBy ?? string.Empty, orderDesc, page, pageSize);
    }

    public Task<T> Add(T item)
        => _store.AddOne(item);

    public Task<T> Update(T item)
        => _store.UpdateOne(item);

    public Task Delete(string key)
        => _store.DeleteOne(key);
    
    public Task Purge(string key)
        => _store.PurgeOne(key);

    public Task Restore(string key)
        => _store.RestoreOne(key);

    public Task<IEnumerable<T>> Add(IEnumerable<T> item)
        => _store.AddMany(item.ToList());

    public Task<IEnumerable<T>> Update(IEnumerable<T> item)
        => _store.UpdateMany(item.ToList());

    public Task Delete(IEnumerable<string> keys)
        => _store.DeleteMany(keys);

    public Task Purge(IEnumerable<string> keys)
        => _store.PurgeMany(keys);

    public Task Restore(IEnumerable<string> keys)
        => _store.RestoreMany(keys);
}
using UntoMeWorld.Domain.Common;

namespace UntoMeWorld.WasmClient.Client.Services.Base;

public interface IService<T>
{
    public Task<PaginationResult<T>> Paginate(QueryFilter filter = null, string orderBy = null, bool orderDesc = false,
        int page = 1,
        int pageSize = 30);
    
    public Task<PaginationResult<T>> PaginateDeleted(QueryFilter filter = null, string orderBy = null, bool orderDesc = false,
        int page = 1,
        int pageSize = 30);

    public Task<T> Add(T item);
    public Task<T> Update(T item);
    public Task Delete(string key);
    public Task Purge(string key);
    public Task Restore(string key);
    
    public Task<IEnumerable<T>> Add(IEnumerable<T> item);
    public Task<IEnumerable<T>> Update(IEnumerable<T> item);
    public Task Delete(IEnumerable<string> keys);
    public Task Purge(IEnumerable<string> keys);
    public Task Restore(IEnumerable<string> keys);
}
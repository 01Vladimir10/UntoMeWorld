using UntoMeWorld.Domain.Common;

namespace UntoMeWorld.Application.Services.Base;

public interface IService <T, in TKey>
{
    public Task<T> Add(T item);
    public Task<T> Get(TKey id);
    public Task<T> Update(T item);
    public Task Restore(TKey item);
    public Task Delete(TKey id, bool softDelete = true);
    public Task<IEnumerable<T>> GetAll(string? query = null);
    public Task<IEnumerable<T>> Add(IEnumerable<T> item);
    public Task<PaginationResult<T>> 
        Query(QueryFilter? filter = null, 
            string? textSearch = null,
            string? orderBy = null, bool orderDesc = false, int page = 1, int pageSize = 100);
    public Task<IEnumerable<T>> Update(IEnumerable<T> item);
    public Task Restore(IEnumerable<TKey> item);
    public Task Delete(IEnumerable<TKey> id, bool softDelete = true);
}
public interface IService<T> : IService<T, string> {

}
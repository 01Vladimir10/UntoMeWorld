using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Stores;

namespace UntoMeWorld.WasmClient.Client.Data.Repositories;

public interface IRepository<T>
{
    public Task<T> Add(T item);
    public Task<T> Update(T item);
    public Task Delete(T item);
    public Task<PaginationResult<T>> Query(string query = null, string sortBy = "", bool sortDesc = false, int page = 1, int pageSize = 50);
    public Task<IEnumerable<T>> Find(Predicate<T> query);
    public Task<IEnumerable<T>> All();
}
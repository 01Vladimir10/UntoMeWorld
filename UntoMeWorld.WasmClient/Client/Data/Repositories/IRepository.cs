namespace UntoMeWorld.WasmClient.Client.Data.Repositories;

public interface IRepository<T>
{
    public Task<T> Add(T item);
    public Task<T> Update(T item);
    public Task Delete(T item);
    public Task<IEnumerable<T>> All(string query = null, string sortBy = "", bool sortDesc = false);
    public Task<IEnumerable<T>> All(Predicate<T> query);
}
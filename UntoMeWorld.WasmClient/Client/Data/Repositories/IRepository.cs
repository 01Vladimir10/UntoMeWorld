namespace UntoMeWorld.WasmClient.Client.Data.Repositories;

public interface IRepository<T>
{
    public Task<T?> Add(T item);
    public Task<T?> Update(T item);
    public Task Delete(T item);
    public Task<IEnumerable<T>> All();
    public Task<IEnumerable<T>> All(string query);
    public Task<IEnumerable<T>> All(Predicate<T> query);
}
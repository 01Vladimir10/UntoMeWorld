namespace UntoMeWorld.WasmClient.Server.Services;

public interface IDatabaseService <T, in TKey>
{
    public Task<T> Add(T item);
    public Task<T> Get(TKey id);
    public Task<T> Update(T item);
    public Task Delete(TKey id);
    public Task<IEnumerable<T>> GetAll(string query = null);
    public Task<IEnumerable<T>> Add(IEnumerable<T> item);
    public Task<IEnumerable<T>> Update(IEnumerable<T> item);
    public Task Delete(IEnumerable<TKey> id);
}
namespace UntoMeWorld.WasmClient.Server.Services.Base;

public interface ISecurityService<T>
{
    public Task<T> Add(T item);
    public Task<T> Get(string id);
    public Task<List<T>> GetAll();
    public Task<T> Update(T item);
    public Task<IEnumerable<T>> Update(IEnumerable<T> items);
    public Task Delete(params string[] ids);
}
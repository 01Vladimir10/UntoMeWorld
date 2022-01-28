using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Services.Abstractions;

public abstract class GenericSecurityService<T> : ISecurityService<T> where T : IModel
{
    protected readonly ISecurityStore<T> Store;

    protected GenericSecurityService(ISecurityStore<T> store)
    {
        Store = store;
    }

    public Task<T> Get(string id) => Store.Get(id);

    public Task<List<T>> GetAll() => Store.All();

    public async Task<T> Update(T item)
    {
        await Store.Update(item);
        return item;
    }

    public async Task<IEnumerable<T>> Update(IEnumerable<T> items)
    {
        var arr = items.ToArray();
        await Store.Update(arr);
        return arr;
    }

    public Task Delete(params string[] ids)
        => Store.Delete(ids);
}
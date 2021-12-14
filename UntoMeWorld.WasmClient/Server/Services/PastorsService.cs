using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;

namespace UntoMeWorld.WasmClient.Server.Services;

public class PastorsService : IDatabaseService<Pastor, string>
{
    private readonly IPastorsStore _store;

    public PastorsService(IPastorsStore store)
    {
        _store = store;
    }

    public Task<Pastor> Add(Pastor item)
    {
        return _store.Add(item);
    }

    public Task<Pastor> Get(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Pastor> Update(Pastor item)
    {
        return _store.Update(item);
    }

    public Task Delete(string id)
    {
        return _store.Delete(new Pastor {Id = id});
    }

    public Task<IEnumerable<Pastor>> GetAll(string query = null)
    {
        return string.IsNullOrEmpty(query) ? _store.All() :  _store.All(query);
    }

    public Task<IEnumerable<Pastor>> Add(IEnumerable<Pastor> item)
    {
        return _store.Add(item.ToList());
    }

    public Task<IEnumerable<Pastor>> Update(IEnumerable<Pastor> item)
    {
        return _store.Update(item.ToList());
    }

    public Task Delete(IEnumerable<string> ids)
    {
        return _store.Delete(ids.Select(id => new Pastor {Id = id}).ToList());
    }
}
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;

namespace UntoMeWorld.WasmClient.Server.Services;

public class ChurchesService : IDatabaseService<Church, string>
{
    private readonly IChurchesStore _churchStore;
    public ChurchesService(IChurchesStore churchStore)
    {
        _churchStore = churchStore;
    }

    public Task<Church> Add(Church item)
    {
        return _churchStore.Add(item);
    }

    public Task<Church> Get(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Church> Update(Church item)
    {
        return _churchStore.Update(item);
    }

    public Task Delete(string id)
    {
        return _churchStore.Delete(new Church { Id = id });
    }

    public Task<IEnumerable<Church>> GetAll(string query = null)
    {
        return string.IsNullOrEmpty(query) ? _churchStore.All() : _churchStore.All(query);
    }

    public Task<IEnumerable<Church>> Add(IEnumerable<Church> item)
    {
        return _churchStore.Add(item.ToList());
    }

    public Task<IEnumerable<Church>> Update(IEnumerable<Church> item)
    {
        return _churchStore.Update(item.ToList());
    }

    public Task Delete(IEnumerable<string> ids)
    {
        return _churchStore.Add(ids.Select(id => new Church {Id = id}).ToList());
    }
}
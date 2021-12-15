using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;

namespace UntoMeWorld.WasmClient.Server.Services;

public class ChildrenService : IDatabaseService<Child, string>
{
    private readonly IChildrenStore _childrenStore;

    public ChildrenService(IChildrenStore childrenStore)
    {
        _childrenStore = childrenStore;
    }

    public Task<Child> Add(Child item)
    {
        return _childrenStore.Add(item);
    }

    public Task<Child> Get(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Child> Update(Child item)
    {
        return _childrenStore.Update(item);
    }

    public Task Delete(string id)
    {
        return _childrenStore.Delete(new Child { Id = id });
    }

    public Task<IEnumerable<Child>> GetAll(string query = null)
    {
        return _childrenStore.All(query);
    }

    public Task<IEnumerable<Child>> Add(IEnumerable<Child> item)
    {
        return _childrenStore.Add(item.ToList());
    }

    public Task<IEnumerable<Child>> Update(IEnumerable<Child> item)
    {
        return _childrenStore.Update(item.ToList());
    }

    public Task Delete(IEnumerable<string> id)
    {
        return _childrenStore.Add(id.Select(i => new Child {Id = i}).ToList());
    }
}
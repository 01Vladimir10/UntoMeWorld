using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;

namespace UntoMeWorld.WasmClient.Server.Services;

public class ChildrenService : GenericDatabaseService<Child>
{
    public ChildrenService(IChildrenStore store) : base(store)
    {
    }
}
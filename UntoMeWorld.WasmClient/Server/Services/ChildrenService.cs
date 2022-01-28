using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Services.Abstractions;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Services;

public class ChildrenService : GenericDatabaseService<Child>, IChildrenService
{
    public ChildrenService(IChildrenStore store) : base(store)
    {
    }
}
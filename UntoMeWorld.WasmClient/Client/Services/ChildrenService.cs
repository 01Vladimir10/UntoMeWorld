using UntoMeWorld.Domain.Model;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.WasmClient.Client.Services.Base;

namespace UntoMeWorld.WasmClient.Client.Services;

public class ChildrenService : GenericService<Child>, IChildrenService
{
    public ChildrenService(IChildrenStore store) : base(store)
    {
    }
}
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.WasmClient.Client.Services.Base;

namespace UntoMeWorld.WasmClient.Client.Services;

public class PastorsService : GenericService<Pastor>, IPastorService
{
    public PastorsService(IPastorsStore store) : base(store)
    {
        
    }
}
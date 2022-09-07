using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Services.Abstractions;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Services;

public class PastorsService : GenericDatabaseService<Pastor>, IPastorsService
{
    public PastorsService(IPastorsStore store) : base(store)
    {
        
    }
}
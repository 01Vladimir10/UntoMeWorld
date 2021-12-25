using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;

namespace UntoMeWorld.WasmClient.Server.Services;

public class PastorsService : GenericDatabaseService<Pastor>
{
    public PastorsService(IPastorsStore store) : base(store)
    {
        
    }
}
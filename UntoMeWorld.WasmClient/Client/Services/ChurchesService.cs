using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Client.Services.Base;

namespace UntoMeWorld.WasmClient.Client.Services;

public class ChurchesService : GenericService<Church>, IChurchesService
{
    public ChurchesService(IChurchesStore store) : base(store)
    {
        
    }
}
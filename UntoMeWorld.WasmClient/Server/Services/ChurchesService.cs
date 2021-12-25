using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;

namespace UntoMeWorld.WasmClient.Server.Services;

public class ChurchesService : GenericDatabaseService<Church>
{
    public ChurchesService(IChurchesStore store) : base(store)
    {
    }
}
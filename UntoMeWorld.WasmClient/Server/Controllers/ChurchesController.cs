using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Services;

namespace UntoMeWorld.WasmClient.Server.Controllers;

public class ChurchesController : GenericController<Church, string>
{
    public ChurchesController(IDatabaseService<Church, string> databaseService) : base(databaseService)
    {
        
    }
}
using Microsoft.AspNetCore.Authorization;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Services;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[Authorize]
public class ChurchesController : GenericController<Church, string>
{
    public ChurchesController(IChurchesService databaseService) : base(databaseService)
    {
        
    }
}
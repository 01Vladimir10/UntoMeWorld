using Microsoft.AspNetCore.Authorization;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Controllers;

public class ChurchesController : GenericController<Church, string>
{
    public ChurchesController(IChurchesService databaseService) : base(databaseService)
    {
        
    }
}
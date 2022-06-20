using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Common;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Controllers;


[ResourceName(ResourceType.Churches)]
public class ChurchesController : GenericController<Church, string>
{
    public ChurchesController(IChurchesService databaseService) : base(databaseService)
    {
        
    }
}
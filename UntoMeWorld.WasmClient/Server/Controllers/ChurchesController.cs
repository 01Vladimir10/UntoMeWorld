using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Shared.DTOs.Church;

namespace UntoMeWorld.WasmClient.Server.Controllers;


[ResourceName(ApiResource.Churches)]
public class ChurchesController : GenericController<Church, ChurchDto, UpdateChurchDto>
{
    public ChurchesController(IChurchesService databaseService) : base(databaseService)
    {
        
    }
}
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Common;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Controllers;


[ResourceName(ResourceType.Children)]
public class PastorsController : GenericController<Pastor, string>
{
    public PastorsController(IPastorsService databaseService) : base(databaseService)
    {
        
    }
}
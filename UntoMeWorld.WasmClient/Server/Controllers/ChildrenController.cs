
using Microsoft.AspNetCore.Authorization;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Common;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[Authorize]
[ResourceName(ResourceType.Children)]
public class ChildrenController : GenericController<Child, string>
{
    public ChildrenController(IChildrenService databaseService) : base(databaseService)
    {
        
    }

}
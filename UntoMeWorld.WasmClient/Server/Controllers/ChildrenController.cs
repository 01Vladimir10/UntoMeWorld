
using Microsoft.AspNetCore.Authorization;
using UntoMeWorld.Application.Services;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using UntoMeWorld.WasmClient.Shared.DTOs.Children;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[Authorize]
[ResourceName(ApiResource.Children)]
public class ChildrenController : GenericController<Child, ChildDto, UpdateChildDto>
{
    public ChildrenController(IChildrenService databaseService) : base(databaseService)
    {
        
    }
}
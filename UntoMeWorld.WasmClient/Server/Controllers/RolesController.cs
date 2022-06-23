using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Shared.Model;
using UntoMeWorld.WasmClient.Shared.Security.Utils;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[Authorize]
[ResourceName(ApiResource.Children)]
public class RolesController : BaseController
{
    private readonly IRolesService _service;
    
    public RolesController(IRolesService service)
    {
        _service = service;
    }

    [HttpGet("{roleId}")]
    [RequiredPermission(PermissionType.Read)]
    public Task<ActionResult<ResponseDto<Role>>> GetRole(string roleId) =>
        ServiceCallResult(() => _service.Get(roleId));

    [HttpGet]
    [RequiredPermission(PermissionType.Read)]
    public Task<ActionResult<ResponseDto<List<Role>>>> GetRoles() =>
        ServiceCallResult(() => Task.FromResult(new List<Role>()));

    [HttpGet("Permissions")]
    [RequiredPermission(PermissionType.Read)]
    public Task<ActionResult<ResponseDto<Dictionary<string, Permission>>>> GetCurrentUserPermissions() =>
        ServiceCallResult(
            async () =>
            {
                var roles = await _service.GetRoleByUser(User.Claims.ToAppUser()?.Id);
                var permissions = await _service.GetEffectivePermissionByRole(roles.Select(r => r.Name));
                return permissions;
            });
}
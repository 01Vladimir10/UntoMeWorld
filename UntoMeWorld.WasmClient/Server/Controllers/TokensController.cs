using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Application.Extensions.Security;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[ApiController]
[Authorize("UserAuthenticationOnly")]
[Route("api/[controller]")]
[ResourceName(ApiResource.Tokens)]
public class TokensController : ControllerBase
{
    private readonly ITokensService _factory;

    public TokensController(ITokensService factory)
    {
        _factory = factory;
    }

    [HttpGet("add")]
    [RequiredPermission(PermissionType.Add)]
    public async Task<ActionResult<ResponseDto<string>>> Add()
    {
        var currentUser = HttpContext.User.Claims.ToAppUser();
        var token = await _factory.Add(currentUser, new Token
        {
            Description = "Test",
            Roles = currentUser.Roles,
            ExpiresOn = DateTime.UtcNow.AddDays(15)
        });
        return new JsonResult(ResponseDto<string>.Successful(token));
    }
}
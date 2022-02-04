using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Security.Utils;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[ApiController]
[Authorize("UserAuthenticationOnly")]
[Route("api/[controller]")]
public class TokensController : ControllerBase
{
    private readonly ITokensService _factory;

    public TokensController(ITokensService factory)
    {
        _factory = factory;
    }

    [HttpGet("add")]
    public async Task<ActionResult<ResponseDto<string>>> Add()
    {
        var currentUser = HttpContext.User.Claims.ToAppUser();
        var token = await _factory.Add(currentUser, new Token
        {
            Description = "Test",
            Roles = currentUser.Roles,
            ExpiresOn = DateTime.Now.AddMinutes(3)
        });
        return new JsonResult(ResponseDto<string>.Successful(token));
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.WasmClient.Server.Security.Utils;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[Authorize("UserAuthenticationOnly")]
[ApiController]
[Route("api/[controller]")]
public class TokensController : ControllerBase
{
    private readonly IJwtTokenFactory _factory;

    public TokensController(IJwtTokenFactory factory)
    {
        _factory = factory;
    }

    [HttpGet("add")]
    public async Task<ActionResult<ResponseDto<string>>> Add()
    {
        var token = await _factory.GenerateToken(HttpContext.User.Claims.ToAppUser(), "test");
        return new JsonResult(ResponseDto<string>.Successful(token));
    }
}
using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Domain.Errors;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : Controller
{
    protected async Task<IActionResult> ServiceCallResult<TResponse>(
        Func<Task<TResponse>> func)
    {
        try
        {
            var result = await func();
            return Ok(result);
        }
        catch (Exception e)
        {
            return e is UserErrorException userError
                ? BadRequestError(userError)
                : InternalServerError(e);
        }
    }

    protected static IActionResult InternalServerError(Exception error)
        => new ObjectResult(new ErrorDto { Error = error.Message, Cause = "", Fix = "Contact support"})
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

    protected static IActionResult BadRequestError(UserErrorException e)
        => new ObjectResult(new ErrorDto(e))
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
}
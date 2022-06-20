using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Errors;
using UntoMeWorld.WasmClient.Shared.Errors;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected static async Task<ActionResult<ResponseDto<TResponse>>> ServiceCallResult<TResponse>(
        Func<Task<TResponse>> func)
    {
        ResponseDto<TResponse> response;
        try
        {
            var result = await func();
            response = ResponseDto<TResponse>.Successful(result);
        }
        catch (InvalidApiRequestRequestException exception)
        {
            return ResponseDto<TResponse>.Error(exception.Message);
        }
        catch (InvalidQueryFilterException exception)
        {
            return ResponseDto<TResponse>.Error(exception.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error occured while executing request: {0}", e);
            return new StatusCodeResult(500);
        }

        return new JsonResult(response);
    }
}
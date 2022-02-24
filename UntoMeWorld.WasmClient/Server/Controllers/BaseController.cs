using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Errors;
using UntoMeWorld.WasmClient.Shared.Errors;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<T, TKey> : ControllerBase
{
    [HttpGet("{id}")]
    public abstract Task<ActionResult<ResponseDto<T>>> Get(TKey id);

    [HttpPost]
    public abstract Task<ActionResult<ResponseDto<T>>> Add(T item);

    [HttpDelete("{id}")]
    public abstract Task<ActionResult<ResponseDto<bool>>> Delete(TKey id);

    [HttpPut]
    public abstract Task<ActionResult<ResponseDto<T>>> Update(T item);

    [HttpPost("query")]
    public abstract Task<ActionResult<ResponseDto<PaginationResult<T>>>> Query(QueryRequestDto query);

    [HttpPost("bulk/insert")]
    public abstract Task<ActionResult<ResponseDto<IEnumerable<T>>>> BulkInsert(List<T> items);

    [HttpPost("bulk/update")]
    public abstract Task<ActionResult<ResponseDto<IEnumerable<T>>>> BulkUpdate(List<T> items);

    [HttpPost("bulk/delete")]
    public abstract Task<ActionResult<ResponseDto<bool>>> BulkDelete(List<TKey> itemIds);
    
    [HttpPut("bin/{id}")]
    public abstract Task<ActionResult<ResponseDto<bool>>> Restore(TKey id);

    [HttpDelete("bin/{id}")]
    public abstract Task<ActionResult<ResponseDto<bool>>> PermanentlyDelete(TKey id);
    
    [HttpPost("bin/bulk/restore")]
    public abstract Task<ActionResult<ResponseDto<bool>>> Restore(IEnumerable<TKey> ids);

    [HttpPost("bin/bulk/delete")]
    public abstract Task<ActionResult<ResponseDto<bool>>> PermanentlyDelete(IEnumerable<TKey> ids);

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
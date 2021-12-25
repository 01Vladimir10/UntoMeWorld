using System.Net;
using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Shared.Errors;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<T, TKey> : ControllerBase
{
    [HttpPost]
    public abstract Task<ActionResult<ResponseDto<T>>> Add(T item);
        
    [HttpDelete]
    public abstract Task<ActionResult<ResponseDto<bool>>> Delete(TKey itemId);
        
    [HttpPut]
    public abstract Task<ActionResult<ResponseDto<T>>> Update(T item);
        
    [HttpGet]
    public abstract Task<ActionResult<ResponseDto<PaginationResult<T>>>> All(string query = null, string sortBy = "", bool sortDesc = false, int page = 1, int pageSize = 100);

    [HttpPost("bulk")]
    public abstract Task<ActionResult<ResponseDto<IEnumerable<T>>>> BulkInsert(List<T> items);
        
    [HttpPut("bulk")]
    public abstract Task<ActionResult<ResponseDto<IEnumerable<T>>>> BulkUpdate(List<T> items);
    [HttpDelete("bulk")]
    public abstract Task<ActionResult<ResponseDto<bool>>> BulkDelete(List<TKey> itemId);
        
    protected static async Task<ActionResult<ResponseDto<TResponse>>> ServiceCallResult<TResponse>(Func<Task<TResponse>> func)
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
        catch (Exception e)
        {
            Console.WriteLine("Error occured while executing request: {0}", e);
            return new StatusCodeResult(500);
        }
        return new JsonResult(response);
    }
}
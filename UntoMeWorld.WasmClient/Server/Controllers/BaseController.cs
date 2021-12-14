using Microsoft.AspNetCore.Mvc;
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
    public abstract Task<ActionResult<ResponseDto<IEnumerable<T>>>> All(string? query = null);

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
            response =  ResponseDto<TResponse>.Successful(result);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
            response = ResponseDto<TResponse>.Error(e.Message);
        }
        return new JsonResult(response);
    }
}
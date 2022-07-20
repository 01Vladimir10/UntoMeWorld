using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Server.Controllers;

public abstract class ApiController<T, TKey> : BaseController
{
    [HttpGet("{id}")]
    public abstract Task<IActionResult> Get(TKey id);

    [HttpPost]
    public abstract Task<IActionResult> Add(T item);

    [HttpDelete("{id}")]
    public abstract Task<IActionResult> Delete(TKey id);

    [HttpPut]
    public abstract Task<IActionResult> Update(T item);

    [HttpPost("query")]
    public abstract Task<IActionResult> Query(QueryRequestDto query);

    [HttpPost("bulk/insert")]
    public abstract Task<IActionResult> BulkInsert(List<T> items);

    [HttpPost("bulk/update")]
    public abstract Task<IActionResult> BulkUpdate(List<T> items);

    [HttpPost("bulk/delete")]
    public abstract Task<IActionResult> BulkDelete(List<TKey> itemIds);
    
    [HttpPut("bin/{id}")]
    public abstract Task<IActionResult> Restore(TKey id);

    [HttpDelete("bin/{id}")]
    public abstract Task<IActionResult> PermanentlyDelete(TKey id);
    
    [HttpPost("bin/bulk/restore")]
    public abstract Task<IActionResult> Restore(IEnumerable<TKey> ids);

    [HttpPost("bin/bulk/delete")]
    public abstract Task<IActionResult> PermanentlyDelete(IEnumerable<TKey> ids);
}
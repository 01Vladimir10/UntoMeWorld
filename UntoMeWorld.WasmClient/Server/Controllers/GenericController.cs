using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Server.Controllers;

public abstract class GenericController<TModel, TKey> : BaseController<TModel, TKey>
{
    private const int PageSize = 100;
    protected readonly IDatabaseService<TModel, TKey> DatabaseService;

    protected GenericController(IDatabaseService<TModel, TKey> databaseService)
    {
        DatabaseService = databaseService;
    }

    public override Task<ActionResult<ResponseDto<PaginationResult<TModel>>>> Query(QueryRequestDto query)
        => ServiceCallResult(() =>
            DatabaseService.Query(query.Filter, query.OrderBy, query.OrderDesc, query.Page, query.PageSize));

    public override Task<ActionResult<ResponseDto<TModel>>> Add(TModel item)
    {
        return ServiceCallResult(() => DatabaseService.Add(item));
    }

    public override Task<ActionResult<ResponseDto<bool>>> Delete(TKey id)
    {
        
        return ServiceCallResult(async () =>
        {
            await DatabaseService.Delete(id);
            return true;
        });
    }

    public override Task<ActionResult<ResponseDto<TModel>>> Update(TModel item)
    {
        return ServiceCallResult(() => DatabaseService.Update(item));
    }

    public override Task<ActionResult<ResponseDto<IEnumerable<TModel>>>> BulkInsert(List<TModel> items)
    {
        return ServiceCallResult(() => DatabaseService.Add(items));
    }

    public override Task<ActionResult<ResponseDto<IEnumerable<TModel>>>> BulkUpdate(List<TModel> items)
    {
        return ServiceCallResult(() => DatabaseService.Update(items));
    }


    public override Task<ActionResult<ResponseDto<bool>>> Restore(TKey id)
        => ServiceCallResult(async () =>
        {
            await DatabaseService.Restore(id);
            return true;
        });
    public override Task<ActionResult<ResponseDto<bool>>> Restore(IEnumerable<TKey> ids)
        => ServiceCallResult(async () =>
        {
            await DatabaseService.Restore(ids);
            return true;
        });

    public override Task<ActionResult<ResponseDto<bool>>> PermanentlyDelete(TKey id)
        => ServiceCallResult(async () =>
        {
            await DatabaseService.Delete(id, false);
            return true;
        });
    public override Task<ActionResult<ResponseDto<bool>>> PermanentlyDelete(IEnumerable<TKey> ids)
        => ServiceCallResult(async () =>
        {
            await DatabaseService.Delete(ids, false);
            return true;
        });
    
    public override Task<ActionResult<ResponseDto<bool>>> BulkDelete(List<TKey> ids)
        => ServiceCallResult(async () =>
        {
            await DatabaseService.Delete(ids);
            return true;
        });
}
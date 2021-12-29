using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Services;
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

    public override Task<ActionResult<ResponseDto<PaginationResult<TModel>>>> All(string query = null, string sortBy = "", bool sortDesc = false, int page = 1, int pageSize = PageSize)
        => ServiceCallResult(() => DatabaseService.Query(query, sortBy, sortDesc,  false,page, pageSize));

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
    
    public override Task<ActionResult<ResponseDto<PaginationResult<TModel>>>> QueryDeletedElements(string query = null, string sortBy = "", bool sortDesc = false, int page = 1,
        int pageSize = 100)
        => ServiceCallResult(() => DatabaseService.Query(query, sortBy, sortDesc,  true,page, pageSize));
}
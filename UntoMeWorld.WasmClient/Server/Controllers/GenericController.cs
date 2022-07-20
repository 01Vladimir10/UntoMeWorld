using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Shared.Errors;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Server.Controllers;

public abstract class GenericController<TModel, TKey> : ApiController<TModel, TKey>
{
    protected readonly IDatabaseService<TModel, TKey> DatabaseService;

    protected GenericController(IDatabaseService<TModel, TKey> databaseService)
    {
        DatabaseService = databaseService;
    }

    [RequiredPermission(PermissionType.Read)]
    public override Task<IActionResult> Query(QueryRequestDto query)
        => ServiceCallResult(() =>
        {
            query.Validate<TModel>();
            return DatabaseService.Query(query.Filter, query.OrderBy, query.OrderDesc, query.Page, query.PageSize);
        });

    [RequiredPermission(PermissionType.Add)]
    public override Task<IActionResult> Add(TModel item)
    {
        return ServiceCallResult(() => DatabaseService.Add(item));
    }

    [RequiredPermission(PermissionType.Delete)]
    public override Task<IActionResult> Delete(TKey id)
    {
        return ServiceCallResult(async () =>
        {
            await DatabaseService.Delete(id);
            return true;
        });
    }


    [RequiredPermission(PermissionType.Update)]
    public override Task<IActionResult> Update(TModel item)
    {
        return ServiceCallResult(() => DatabaseService.Update(item));
    }


    [RequiredPermission(PermissionType.Add)]
    public override Task<IActionResult> BulkInsert(List<TModel> items)
    {
        return ServiceCallResult(() => DatabaseService.Add(items));
    }

    [RequiredPermission(PermissionType.Update)]
    public override Task<IActionResult> BulkUpdate(List<TModel> items)
    {
        return ServiceCallResult(() => DatabaseService.Update(items));
    }


    [RequiredPermission(PermissionType.Restore)]
    public override Task<IActionResult> Restore(TKey id)
        => ServiceCallResult(async () =>
        {
            await DatabaseService.Restore(id);
            return true;
        });


    [RequiredPermission(PermissionType.Restore)]
    public override Task<IActionResult> Restore(IEnumerable<TKey> ids)
        => ServiceCallResult(async () =>
        {
            await DatabaseService.Restore(ids);
            return true;
        });


    [RequiredPermission(PermissionType.Purge)]
    public override Task<IActionResult> PermanentlyDelete(TKey id)
        => ServiceCallResult(async () =>
        {
            await DatabaseService.Delete(id, false);
            return true;
        });


    [RequiredPermission(PermissionType.Purge)]
    public override Task<IActionResult> PermanentlyDelete(IEnumerable<TKey> ids)
        => ServiceCallResult(async () =>
        {
            await DatabaseService.Delete(ids, false);
            return true;
        });

    [RequiredPermission(PermissionType.Delete)]
    public override Task<IActionResult> BulkDelete(List<TKey> itemIds)
        => ServiceCallResult(async () =>
        {
            await DatabaseService.Delete(itemIds);
            return true;
        });


    [RequiredPermission(PermissionType.Read)]
    public override Task<IActionResult> Get(TKey id)
        => ServiceCallResult(() => DatabaseService.Get(id));
}
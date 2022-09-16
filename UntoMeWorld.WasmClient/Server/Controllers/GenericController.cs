using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using UntoMeWorld.WasmClient.Shared.DTOs;
using UntoMeWorld.WasmClient.Shared.Model;
namespace UntoMeWorld.WasmClient.Server.Controllers;

public abstract class GenericController<TModel, TAddDto, TUpdateDto> : ApiController<TModel, TAddDto, TUpdateDto, string>
    where TModel : IModel 
    where TAddDto : IDto<TModel> 
    where TUpdateDto : IUpdateDto<TModel>
{
    protected readonly IService<TModel, string> Service;

    protected GenericController(IService<TModel, string> service)
    {
        Service = service;
    }

    [RequiredPermission(PermissionType.Read)]
    public override Task<IActionResult> Query(QueryRequestDto query)
        => ServiceCallResult(() =>
        {
            query.Validate<TModel>();
            return Service.Query(query.Filter, query.TextQuery, query.OrderBy, query.OrderDesc, query.Page, query.PageSize);
        });

    [RequiredPermission(PermissionType.Add)]
    public override Task<IActionResult> Add(TAddDto item)
    {
        return ServiceCallResult(() => Service.Add(item.ToModel()));
    }

    [RequiredPermission(PermissionType.Delete)]
    public override Task<IActionResult> Delete(string id)
    {
        return ServiceCallResult(async () =>
        {
            await Service.Delete(id);
            return true;
        });
    }


    [RequiredPermission(PermissionType.Update)]
    public override Task<IActionResult> Update(TUpdateDto item)
    {
        return ServiceCallResult(() => Service.Update(item.ToModel()));
    }


    [RequiredPermission(PermissionType.Add)]
    public override Task<IActionResult> BulkInsert(IEnumerable<TAddDto> items)
    {
        return ServiceCallResult(() => Service.Add(items.Select(i => i.ToModel())));
    }

    [RequiredPermission(PermissionType.Update)]
    public override Task<IActionResult> BulkUpdate(IEnumerable<TUpdateDto> items)
    {
        return ServiceCallResult(() => Service.Update(items.Select(i => i.ToModel())));
    }


    [RequiredPermission(PermissionType.Restore)]
    public override Task<IActionResult> Restore(string id)
        => ServiceCallResult(async () =>
        {
            await Service.Restore(id);
            return true;
        });


    [RequiredPermission(PermissionType.Restore)]
    public override Task<IActionResult> Restore(IEnumerable<string> ids)
        => ServiceCallResult(async () =>
        {
            await Service.Restore(ids);
            return true;
        });


    [RequiredPermission(PermissionType.Purge)]
    public override Task<IActionResult> PermanentlyDelete(string id)
        => ServiceCallResult(async () =>
        {
            await Service.Delete(id, false);
            return true;
        });


    [RequiredPermission(PermissionType.Purge)]
    public override Task<IActionResult> PermanentlyDelete(IEnumerable<string> ids)
        => ServiceCallResult(async () =>
        {
            await Service.Delete(ids, false);
            return true;
        });

    [RequiredPermission(PermissionType.Delete)]
    public override Task<IActionResult> BulkDelete(List<string> itemIds)
        => ServiceCallResult(async () =>
        {
            await Service.Delete(itemIds);
            return true;
        });


    [RequiredPermission(PermissionType.Read)]
    public override Task<IActionResult> Get(string id)
        => ServiceCallResult(() => Service.Get(id));
}
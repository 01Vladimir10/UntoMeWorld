using Microsoft.AspNetCore.Components.Web.Virtualization;
using UntoMeWorld.Application.Common;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.WasmClient.Shared.Model;
using static UntoMeWorld.Domain.Common.QueryLanguage;

namespace UntoMeWorld.WasmClient.Client.Utils.Extensions;

public static class ServicesExtension
{
    public static async Task<PaginationResult<T>> Paginate<T>(this IService<T> service, QueryRequestDto requestDto)
        => await service.Query(requestDto.Filter, requestDto.TextQuery, requestDto.OrderBy, requestDto.OrderDesc,
            requestDto.Page, requestDto.PageSize);

    public static async Task<PaginationResult<T>> PaginateActive<T>(this IService<T> service,
        QueryRequestDto requestDto)
        => await service.Query(
            requestDto.Filter == null
                ? Eq(nameof(IRecyclableModel.IsDeleted), false)
                : And(requestDto.Filter, Eq(nameof(IRecyclableModel.IsDeleted), false))
            , requestDto.TextQuery, requestDto.OrderBy, requestDto.OrderDesc,
            requestDto.Page, requestDto.PageSize);

    public static async ValueTask<ItemsProviderResult<T>> PaginateAsync<T>(
        this IService<T> service,
        ItemsProviderRequest request,
        int pageSize = 15,
        QueryFilter? query = null,
        string? textSearch = null,
        string? orderBy = null,
        bool orderDesc = true)
    {
        var pageIndex = request.StartIndex < pageSize ? 1 : request.StartIndex / pageSize +  1;
        var page = await service.Query(query, textSearch, orderBy, orderDesc, pageIndex, pageSize);
        return new ItemsProviderResult<T>(page.Result ?? new List<T>(), page.TotalItems);
    }
}
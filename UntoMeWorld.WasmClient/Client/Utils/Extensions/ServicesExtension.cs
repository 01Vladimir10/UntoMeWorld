using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Common;
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
}
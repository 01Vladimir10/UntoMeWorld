using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Client.Utils.Extensions;

public static class ServicesExtension
{
    public static async Task<PaginationResult<T>> Paginate<T>(this IService<T> service, QueryRequestDto requestDto)
        => await service.Query(requestDto.Filter, requestDto.TextQuery, requestDto.OrderBy, requestDto.OrderDesc,
            requestDto.Page, requestDto.PageSize);
}
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.WasmClient.Shared.DTOs;

public interface IUpdateDto<out TModel> : IDto<TModel>, IModel where TModel : IModel
{
}
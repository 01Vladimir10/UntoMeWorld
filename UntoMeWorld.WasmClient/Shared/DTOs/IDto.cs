using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.WasmClient.Shared.DTOs;

public interface IDto<out TModel> where TModel : IModel
{
    public TModel ToModel();
}
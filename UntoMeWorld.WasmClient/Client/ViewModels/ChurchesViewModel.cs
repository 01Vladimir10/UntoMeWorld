using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Services.Base;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public class ChurchesViewModel : BaseViewModel
{
    public List<Church> Churches { get; private set; } = new();

    private readonly IChurchesService _service;

    public ChurchesViewModel(IChurchesService service)
    {
        _service = service;
    }

    public async Task UpdateChurches()
    {
        var churches = await _service.Paginate();
        Churches = churches.Result;
    }
}
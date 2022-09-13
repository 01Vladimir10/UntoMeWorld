using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public class ChurchesViewModel
{
    public List<Church> Churches { get; private set; } = new();

    private readonly IChurchesService _service;

    public ChurchesViewModel(IChurchesService service)
    {
        _service = service;
    }

    public async Task UpdateChurches()
    {
        var churches = await _service.Query();
        Churches = churches.Result;
    }
}
using System.Collections.Concurrent;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Services.Base;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public class ChildrenViewModel : GenericViewModel<Child>
{
    private readonly IService<Church> _churchesRepository;

    public ChildrenViewModel(IService<Child> service, IService<Church> churchesRepository) : base(service)
    {
        _churchesRepository = churchesRepository;
    }

    public async Task UpdateChurches()
    {
        var churches = await _churchesRepository.Paginate();
        ChurchesDictionary = churches.Result.ToDictionary(c => c.Id, c => c);
    }
    public IDictionary<string, Church> ChurchesDictionary { get; set; } = new ConcurrentDictionary<string, Church>();
}
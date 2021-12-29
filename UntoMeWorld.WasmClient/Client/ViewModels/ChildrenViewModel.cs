using System.Collections.Concurrent;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Data.Repositories;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public class ChildrenViewModel : GenericViewModel<Child>
{
    private readonly IRepository<Church> _churchesRepository;

    public ChildrenViewModel(IRepository<Child> repository, IRepository<Church> churchesRepository) : base(repository)
    {
        _churchesRepository = churchesRepository;
    }

    public async Task UpdateChurches()
    {
        var churches = await _churchesRepository.All();
        ChurchesDictionary = churches.Result.ToDictionary(c => c.Id, c => c);
    }
    public IDictionary<string, Church> ChurchesDictionary { get; set; } = new ConcurrentDictionary<string, Church>();
}
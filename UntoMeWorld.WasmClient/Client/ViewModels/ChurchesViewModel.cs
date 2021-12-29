using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Data.Repositories;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public class ChurchesViewModel : GenericViewModel<Church>
{
    private readonly IRepository<Pastor> _pastorsRepository;

    public ChurchesViewModel(IRepository<Church> repository, IRepository<Pastor> pastorsRepository) : base(repository)
    {
        _pastorsRepository = pastorsRepository;
    }

    public List<Pastor> Pastors { get; private set; } = new();
    public IDictionary<string, Pastor> PastorsDictionary => Pastors.ToDictionary(p => p.Id, p => p);

    public async Task UpdatePastors()
    {
        var pastors = await _pastorsRepository.All();
        Pastors = pastors.Result.ToList();
    }
}
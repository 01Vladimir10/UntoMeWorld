using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Services.Base;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public class ChurchesViewModel : GenericViewModel<Church>
{
    private readonly IPastorService _pastorsRepository;

    public ChurchesViewModel(IChurchesService service, IPastorService pastorsRepository) : base(service)
    {
        _pastorsRepository = pastorsRepository;
    }

    public List<Pastor> Pastors { get; private set; } = new();
    public IDictionary<string, Pastor> PastorsDictionary => Pastors.ToDictionary(p => p.Id, p => p);

    public async Task UpdatePastors()
    {
        var pastors = await _pastorsRepository.Paginate();
        Pastors = pastors.Result.ToList();
    }
}
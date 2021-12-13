using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Data.Repositories;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public class ChurchesViewModel : GenericViewModel<Church>
{
    public ChurchesViewModel(IRepository<Church> repository) : base(repository)
    {
    }
}
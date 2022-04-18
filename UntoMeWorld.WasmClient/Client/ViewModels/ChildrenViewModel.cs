using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Client.Services.Base;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public class ChildrenViewModel : GenericViewModel<Child>
{
    public ChildrenViewModel(IService<Child> service) : base(service)
    {
    }
}
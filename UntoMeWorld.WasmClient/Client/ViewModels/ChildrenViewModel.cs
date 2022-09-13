using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public class ChildrenViewModel : GenericViewModel<Child>
{
    public ChildrenViewModel(IService<Child> service) : base(service)
    {
    }
}
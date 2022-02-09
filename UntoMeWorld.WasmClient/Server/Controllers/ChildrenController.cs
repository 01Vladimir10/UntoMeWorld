using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Controllers;

public class ChildrenController : GenericController<Child, string>
{
    public ChildrenController(IChildrenService databaseService) : base(databaseService)
    {
        
    }

}
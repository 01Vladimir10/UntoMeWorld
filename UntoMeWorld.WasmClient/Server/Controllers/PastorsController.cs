using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Services;

namespace UntoMeWorld.WasmClient.Server.Controllers;

public class PastorsController : GenericController<Pastor, string>
{
    public PastorsController(IDatabaseService<Pastor, string> databaseService) : base(databaseService)
    {
        
    }
}
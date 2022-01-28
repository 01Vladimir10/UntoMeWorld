using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Services;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Controllers;

public class PastorsController : GenericController<Pastor, string>
{
    public PastorsController(IPastorsService databaseService) : base(databaseService)
    {
        
    }
}
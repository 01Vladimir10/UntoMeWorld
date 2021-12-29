using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Services;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Server.Controllers;

public class ChildrenController : GenericController<Child, string>
{
    public ChildrenController(IDatabaseService<Child, string> databaseService) : base(databaseService)
    {
        
    }

}
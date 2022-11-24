using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using static UntoMeWorld.Domain.Query.QueryLanguage;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[Authorize]
[ResourceName(ApiResource.LabelReports)]
[Route("api/[controller]")]
public class ReportsController : Controller
{
    private readonly IChildrenService _childrenService;

    public ReportsController(IChildrenService childrenService)
    {
        _childrenService = childrenService;
    }

    [HttpGet("xmasLabels")]
    public async Task<IActionResult> XmasReport() => View(await GetXmasChildren());

    [HttpGet("xmasList")]
    public async Task<IActionResult> XmasList() => View(await GetXmasChildren());

    private async Task<List<Child>> GetXmasChildren()
    {
        var filter = And(
            Eq((Child c) => c.IsDeleted, false),
            Eq((Child c) => c.ReceivesChristmasGift, true)
        );

        var result = await _childrenService.Query(
            filter: filter,
            orderBy: nameof(Child.ChurchId),
            orderDesc: true,
            page: 1,
            pageSize: 1000);
        if (result.Result is null) return new List<Child>();
        return result
            .Result
            .OrderBy(c => c.Church.Name)
            .ThenBy(c => c.Grade >= 7 ? "HS" : "EL")
            .ThenBy(c => c.Gender)
            .ThenBy(c => c.Name)
            .ToList();
    }
}
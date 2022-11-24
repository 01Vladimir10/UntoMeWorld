using System.Dynamic;
using DotLiquid;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Query;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using UntoMeWorld.WasmClient.Shared.DTOs;

namespace UntoMeWorld.WasmClient.Server.Controllers;

[ResourceName(ApiResource.LabelReports)]
public class ReportsController : GenericController<LabelReport, LabelReportDto, UpdateLabelReportDto>
{
    private readonly IChildrenService _childrenService;

    public ReportsController(ILabelReportsService service, IChildrenService childrenService) : base(service)
    {
        _childrenService = childrenService;
    }

    [HttpGet("run/{reportId}")]
    public async Task<IActionResult> Run(string reportId)
    {
        var report = await Service.Get(reportId);
        if (report == null)
            throw new Exception("");

        var result = await _childrenService.Query(report.Query, orderBy: report.OrderBy, orderDesc: report.OrderDesc,
            pageSize: 1000, page: 1);
        var template = Template.Parse(report.Template);
        var items = JsonConvert
            .DeserializeObject<List<ExpandoObject>>(
                JsonConvert.SerializeObject(result.Result,
                    new JsonSerializerSettings
                    {
                        Formatting = Formatting.None,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            );

        return View(new RunReportModel
        {
            Template = template,
            Items = items,
            Styles = report.StyleSheet
        });
    }
}

public class RunReportModel
{
    public Template Template { get; set; }
    public List<ExpandoObject> Items { get; set; }
    public string Styles { get; set; }
}
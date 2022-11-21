using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Security.Authorization.Attributes;
using UntoMeWorld.WasmClient.Shared.DTOs;
namespace UntoMeWorld.WasmClient.Server.Controllers;


[ResourceName(ApiResource.Children)]
public class LabelReportsController : GenericController<LabelReport, LabelReportDto, UpdateLabelReportDto>
{
    public LabelReportsController(ILabelReportsService service) : base(service)
    {
        
    }
}

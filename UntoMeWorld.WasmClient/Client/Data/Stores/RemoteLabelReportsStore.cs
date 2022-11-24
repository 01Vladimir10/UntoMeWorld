using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Shared.DTOs;

namespace UntoMeWorld.WasmClient.Client.Data.Stores;

public class RemoteLabelReportsStore : GenericRemoteStore<LabelReport, LabelReportDto,UpdateLabelReportDto>, ILabelReportsStore
{
    public RemoteLabelReportsStore(HttpClient client) : base(client, "/api/reports")
    {
        
    }
    protected override LabelReportDto ToAddDto(LabelReport model) => new LabelReportDto().From(model);
    protected override UpdateLabelReportDto ToUpdateDto(LabelReport model) => new UpdateLabelReportDto().From(model);
}
using UntoMeWorld.Application.Services.Abstractions;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Application.Services;

public class LabelReportsService : GenericService<LabelReport>, ILabelReportsService
{
    public LabelReportsService(ILabelReportsStore store, ILogsService logs) : base(store, logs, "labelReports")
    {
    }
}
using System.Text.Json;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Query;
using UntoMeWorld.Infrastructure.Services;

namespace UntoMeWorld.Infrastructure.Stores;

public class MongoLabelReportStore : GenericMongoStore<LabelReport>, ILabelReportsStore
{
    public MongoLabelReportStore(MongoDbService service) : base(service, "labelReports")
    {
        
    }
    public override Task<LabelReport> AddOne(LabelReport data)
    {
        FixPrimitives(data.Query);
        return base.AddOne(data);
    }

    public override Task<LabelReport> UpdateOne(LabelReport data)
    {
        FixPrimitives(data.Query);
        return base.UpdateOne(data);
    }

    public override Task<IEnumerable<LabelReport>> AddMany(List<LabelReport> data)
    {
        foreach (var queryFilter in data.Select(d => d.Query))
        {
            FixPrimitives(queryFilter);
        }
        return base.AddMany(data);
    }

    public override Task<IEnumerable<LabelReport>> UpdateMany(List<LabelReport> data)
    {
        foreach (var queryFilter in data.Select(d => d.Query))
        {
            FixPrimitives(queryFilter);
        }
        return base.UpdateMany(data);
    }

    private static void FixPrimitives(QueryFilter filter)
    {
        if (filter.Value is JsonElement element)
        {
            filter.Value = element.ValueKind switch
            {
                JsonValueKind.Number => element.TryGetDecimal(out var decimalValue) ? decimalValue : 0,
                JsonValueKind.String => element.GetString(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => "",
                _ => element
            };
        }
        if(filter.Children?.Any() ?? false) filter.Children.ForEach(FixPrimitives);
    }
}
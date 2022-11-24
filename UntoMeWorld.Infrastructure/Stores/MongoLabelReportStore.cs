using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;
using UntoMeWorld.Application.Common;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Query;
using UntoMeWorld.Infrastructure.Helpers;
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

    public override async Task<PaginationResult<LabelReport>> Query(QueryFilter? filter,
        string? textQuery = null, string? orderBy = null, bool orderByDesc = false,
        int page = 1, int pageSize = 100)
    {
        await Task.Yield();
        var (totalItems, result) =
            await Collection.QueryByPageAndSort<LabelReport, LabelReport>(filter, textQuery, orderBy ?? string.Empty,
                orderByDesc, page, pageSize,
                new List<IPipelineStageDefinition>
                {
                    new BsonDocumentPipelineStageDefinition<LabelReport, LabelReport>(
                        new BsonDocument("$project",
                            new BsonDocument
                            {
                                { nameof(LabelReport.Template), 0 },
                                { nameof(LabelReport.Query), 0 },
                                { nameof(LabelReport.StyleSheet), 0 }
                            }))
                });
        await Task.Delay(1);
        return new PaginationResult<LabelReport>
        {
            Result = result.ToList(),
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
            TotalItems = totalItems,
            Page = page
        };
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

        if (filter.Children?.Any() ?? false) filter.Children.ForEach(FixPrimitives);
    }
}
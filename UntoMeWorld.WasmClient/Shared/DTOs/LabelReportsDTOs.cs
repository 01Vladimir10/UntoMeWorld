using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Query;

namespace UntoMeWorld.WasmClient.Shared.DTOs;

public class LabelReportDto : IDto<LabelReport>
{
    public string Name { get; set; }
    public string Template { get; set; }
    public string StyleSheet { get; set; }
    public string Collection { get; set; }
    public QueryFilter Query { get; set;}
    public string OrderBy { get; set; } = nameof(Name);
    public bool OrderDesc { get; set; }
    public int Skip { get; set; } = -1;
    public int Take { get; set; } = -1;

    public LabelReport ToModel() => new()
    {
        Id = null,
        CreatedOn = DateTime.Now,
        LastUpdatedOn = DateTime.Now,
        Name = Name,
        Template = Template,
        StyleSheet = StyleSheet,
        Collection = Collection,
        Query = Query,
        OrderBy = OrderBy,
        OrderDesc = OrderDesc,
        Skip = Skip,
        Take = Take,
        IsDeleted = false,
        DeletedOn = DateTime.Now,
    };
    public virtual LabelReportDto From(LabelReport model)
    {
        Name = model.Name;
        Template = model.Template;
        StyleSheet = model.StyleSheet;
        Collection = model.Collection;
        Query = model.Query;
        OrderBy = model.OrderBy;
        OrderDesc = model.OrderDesc;
        Skip = model.Skip;
        Take = model.Take;
        return this;
    }
}

public class UpdateLabelReportDto : LabelReportDto, IUpdateDto<LabelReport>
{

    public string Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public new LabelReport ToModel()
    {
        var model = base.ToModel();
        model.Id = Id;
        model.CreatedOn = CreatedOn;
        model.LastUpdatedOn = LastUpdatedOn;
        return model;
    }

    public override UpdateLabelReportDto From(LabelReport model)
    {
        base.From(model);
        Id = model.Id;
        CreatedOn = model.CreatedOn;
        LastUpdatedOn = model.LastUpdatedOn;
        return this;
    }
}
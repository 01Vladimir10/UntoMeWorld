using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Shared.DTOs.Children;

public class UpdateChildDto : ChildDto, IUpdateDto<Child>
{
    public string Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public DateTime DeletedOn { get; set; }
    public override Child ToModel()
    {
        var child = base.ToModel();
        child.Id = Id;
        child.CreatedOn = CreatedOn;
        child.LastUpdatedOn = LastUpdatedOn;
        child.DeletedOn = DeletedOn;
        return child;
    }

    public override UpdateChildDto From(Child child)
    {
        base.From(child);
        Id = child.Id;
        CreatedOn = child.CreatedOn;
        LastUpdatedOn = child.LastUpdatedOn;
        DeletedOn = child.DeletedOn;
        return this;
    }
}
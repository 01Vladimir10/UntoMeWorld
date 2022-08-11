using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Shared.DTOs.Children;

public class UpdateChildDto : ChildDto, IUpdateDto<Child>
{
    public string Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public override Child ToModel()
    {
        var children = base.ToModel();
        children.Id = Id;
        children.CreatedOn = CreatedOn;
        children.DeletedOn = LastUpdatedOn;
        return children;
    }
}
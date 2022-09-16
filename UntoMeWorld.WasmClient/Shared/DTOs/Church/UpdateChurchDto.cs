namespace UntoMeWorld.WasmClient.Shared.DTOs.Church;
using Domain.Model;

public class UpdateChurchDto : ChurchDto, IUpdateDto<Church>
{
    public string Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public override Church ToModel()
    {
        var church = base.ToModel();
        church.Id = Id;
        church.CreatedOn = CreatedOn;
        church.LastUpdatedOn = LastUpdatedOn;
        return church;
    }

    public override UpdateChurchDto From(Church church)
    {
        base.From(church);
        CreatedOn = church.CreatedOn;
        LastUpdatedOn = church.LastUpdatedOn;
        Id = church.Id;
        return this;
    }
}
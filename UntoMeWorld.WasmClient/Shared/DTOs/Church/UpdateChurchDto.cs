namespace UntoMeWorld.WasmClient.Shared.DTOs.Church;

public class UpdateChurchDto : ChurchDto, IUpdateDto<Domain.Model.Church>
{
    public string Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public override Domain.Model.Church ToModel()
    {
        var church = base.ToModel();
        church.Id = Id;
        church.CreatedOn = CreatedOn;
        church.LastUpdatedOn = LastUpdatedOn;
        return church;
    }
}
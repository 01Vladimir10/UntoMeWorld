using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Client.Data.Model;

public class ChurchModel : Church
{
    public ChurchModel(Church church)
    {
        Id = church.Id;
        Name = church.Name;
        PastorId = church.PastorId;
        CreatedOn = church.CreatedOn;
        LastUpdatedOn = church.LastUpdatedOn;
        DeletedOn = church.DeletedOn;
        IsDeleted = church.IsDeleted;
        IsActive = church.IsActive;
    }
    public new Pastor Pastor { get; set; }
}
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Shared.DTOs.Church;

public class ChurchDto : IDto<Domain.Model.Church>
{
    public bool IsActive { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
    public Pastor Pastor { get; set; }

    public virtual Domain.Model.Church ToModel()
        => new()
        {
            Id = null,
            IsActive = IsActive,
            IsDeleted = false,
            CreatedOn = DateTime.Now,
            DeletedOn = DateTime.Now,
            LastUpdatedOn = DateTime.Now,
            Name = Name,
            Address = Address,
            Pastor = Pastor
        };

    public virtual ChurchDto From(Domain.Model.Church church)
    {
        IsActive = church.IsActive;
        Name = church.Name;
        Address = church.Address;
        Pastor = church.Pastor;
        return this;
    }
}
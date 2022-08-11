using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Shared.DTOs.Children;

public class ChildDto : IDto<Child>
{
    public string Name { get; set; }
    public string Lastname { get; set; }
    public Gender Gender { get; set; }
    public int Age { get; set; }
    public int Grade { get; set; }
    public int ShoeSize { get; set; }
    public int TopSize { get; set; }
    public UnderwearSize UnderwearSize { get; set; }
    public UnderwearSize BraSize { get; set; }
    public int WaistSize { get; set; }
    public bool IsSponsored { get; set; }
    public bool ReceivesChristmasGift { get; set; }
    public bool ReceivesUniforms { get; set; }
    public bool ReceivesShoes { get; set; }
    public string ChurchId { get; set; }
    public int UniformsCount { get; set; }
    public bool IsActive { get; set; }
    public string Notes { get; set; }

    public virtual Child ToModel()
    {
        return new Child
        {
            Id = null,
            IsActive = IsActive,
            IsSponsored = IsSponsored,
            IsDeleted = false,
            CreatedOn = DateTime.Now,
            DeletedOn = DateTime.Now,
            LastUpdatedOn = DateTime.Now,
            Grade = Grade,
            ChurchId = ChurchId,
            ShoeSize = ShoeSize,
            TopSize = TopSize,
            WaistSize = WaistSize,
            UnderwearSize = UnderwearSize,
            BraSize = BraSize,
            UniformsCount = UniformsCount,
            ReceivesChristmasGift = ReceivesChristmasGift,
            ReceivesShoes = ReceivesShoes,
            ReceivesUniforms = ReceivesUniforms,
            Name = Name,
            Lastname = Lastname,
            Age = Age,
            Gender = Gender,
            Notes = Notes,
            Church = null
        };
    }

    public virtual ChildDto From(Child child)
    {
        IsActive = child.IsActive;
        IsSponsored = child.IsSponsored;
        Grade = child.Grade;
        ChurchId = child.ChurchId;
        ShoeSize = child.ShoeSize;
        TopSize = child.TopSize;
        WaistSize = child.WaistSize;
        UnderwearSize = child.UnderwearSize;
        BraSize = child.BraSize;
        UniformsCount = child.UniformsCount;
        ReceivesChristmasGift = child.ReceivesChristmasGift;
        ReceivesShoes = child.ReceivesShoes;
        ReceivesUniforms = child.ReceivesUniforms;
        Name = child.Name;
        Lastname = child.Lastname;
        Age = child.Age;
        Gender = child.Gender;
        Notes = child.Notes;
        return this;
    }
}
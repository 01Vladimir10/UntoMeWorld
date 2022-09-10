using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.Domain.Model
{
    public class Child : IModel, IPerson, IRecyclableModel, ICloneable<Child>
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ChurchId { get; set; }

        public bool IsActive { get; set; }
        public bool IsSponsored { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        [Range(1, 13, ErrorMessage = "The grade must be a value between 1 and 13")]
        public int Grade { get; set; }

        [Range(26, 46, ErrorMessage = "The shoe size must be a value between 26 and 46")]
        public int ShoeSize { get; set; }

        [Range(8, 18, ErrorMessage = "The top size must be a value between 8 and 18")]
        public int TopSize { get; set; }

        [Range(18, 40, ErrorMessage = "The waist size must be a value between 26 and 40")]
        public int WaistSize { get; set; }

        public UnderwearSize UnderwearSize { get; set; }
        public UnderwearSize BraSize { get; set; }
        public int UniformsCount { get; set; }
        public bool ReceivesChristmasGift { get; set; }
        public bool ReceivesShoes { get; set; }
        public bool ReceivesUniforms { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "The name of the child is required")]
        public string Name { get; set; }

        public string Lastname { get; set; }

        [Range(4, 25, ErrorMessage = "The age must be a value between 4 and 25")]
        public int Age { get; set; }

        public Gender Gender { get; set; }
        public string Notes { get; set; }
        [BsonIgnore] public Church Church { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(Grade)}: {Grade}, {nameof(ChurchId)}: {ChurchId}, {nameof(ShoeSize)}: {ShoeSize}, {nameof(TopSize)}: {TopSize}, {nameof(WaistSize)}: {WaistSize}, {nameof(UnderwearSize)}: {UnderwearSize}, {nameof(BraSize)}: {BraSize}, {nameof(UniformsCount)}: {UniformsCount}, {nameof(ReceivesChristmasGift)}: {ReceivesChristmasGift}, {nameof(ReceivesUniforms)}: {ReceivesUniforms}, {nameof(Name)}: {Name}, {nameof(Lastname)}: {Lastname}, {nameof(Age)}: {Age}, {nameof(Gender)}: {Gender}";
        }

        public Child Clone()
        {
            var child = (Child)MemberwiseClone();
            child.Church = Church?.Clone();
            return child;
        }
    }
}
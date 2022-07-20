using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.Domain.Model
{
    public class Child : IModel, IPerson, IRecyclableModel
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public bool IsActive { get; set; }
        public bool IsSponsored { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        [Required] [Range(1, 11)] public int Grade { get; set; }
        public string ChurchId { get; set; }
        [Required] [Range(28, 44)] public int ShoeSize { get; set; }
        [Required] [Range(28, 44)] public int TopSize { get; set; }
        [Required] [Range(20, 38)] public int WaistSize { get; set; }
        [Required] public UnderwearSize UnderwearSize { get; set; }
        [Required] public UnderwearSize BraSize { get; set; }
        [Required] public int UniformsCount { get; set; }
        [Required] public bool ReceivesChristmasGift { get; set; }
        [Required] public bool ReceivesShoes { get; set; }
        [Required] public bool ReceivesUniforms { get; set; }
        [Required] [StringLength(255)] public string Name { get; set; }
        [Required] [StringLength(255)] public string Lastname { get; set; }
        [Required] [Range(4, 22)] public int Age { get; set; }
        public Gender Gender { get; set; }
        [StringLength(1000)] public string Notes { get; set; }

        [BsonIgnore] public Church Church { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(Grade)}: {Grade}, {nameof(ChurchId)}: {ChurchId}, {nameof(ShoeSize)}: {ShoeSize}, {nameof(TopSize)}: {TopSize}, {nameof(WaistSize)}: {WaistSize}, {nameof(UnderwearSize)}: {UnderwearSize}, {nameof(BraSize)}: {BraSize}, {nameof(UniformsCount)}: {UniformsCount}, {nameof(ReceivesChristmasGift)}: {ReceivesChristmasGift}, {nameof(ReceivesUniforms)}: {ReceivesUniforms}, {nameof(Name)}: {Name}, {nameof(Lastname)}: {Lastname}, {nameof(Age)}: {Age}, {nameof(Gender)}: {Gender}";
        }
    }
}
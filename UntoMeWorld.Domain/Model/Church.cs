using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.Domain.Model
{
    public class Church : IModel, IRecyclableModel, ICloneable<Church>
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public Pastor Pastor { get; set; }
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Address)}: {Address}, {nameof(Pastor)}: {Pastor}";
        }

        public Church Clone()
        {
            if (MemberwiseClone() is not Church church)
                return new Church();
            church.Address = Address?.Clone();
            church.Pastor = Pastor?.Clone();
            return church;
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Id?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj) => obj is Church church && church.Id == Id;
    }
}
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.Domain.Model
{
    public class Pastor : IModel, IPerson, IRecyclableModel
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Phone)}: {Phone}, {nameof(Name)}: {Name}, {nameof(Lastname)}: {Lastname}, {nameof(Age)}: {Age}, {nameof(Gender)}: {Gender}";
        }
    }
}
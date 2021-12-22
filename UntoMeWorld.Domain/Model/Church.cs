using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UntoMeWorld.Domain.Model
{
    public class Church : IModel
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
        public string PastorId { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Address)}: {Address}, {nameof(PastorId)}: {PastorId}";
        }
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.MongoDatabase.Models
{
    public class MongoChurch : IChurch
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator)), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        
        #nullable enable
        public IAddress? Address { get; set; }
        public string? PastorId { get; set; }
        #nullable disable


        public MongoChurch()
        {
            
        }
        public MongoChurch(IChurch church)
        {
            Id = church.Id;
            Name = church.Name;
            Address = church.Address;
        }
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Address)}: {Address}, {nameof(PastorId)}: {PastorId}";
        }
    }
}
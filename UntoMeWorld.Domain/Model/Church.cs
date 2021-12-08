using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UntoMeWorld.Domain.Model
{
    public class Church
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string PastorId { get; set; }
    }
}
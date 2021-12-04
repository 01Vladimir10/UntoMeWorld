using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.MongoDatabase.Models
{
    public class MongoPastor : IPastor
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Lastname { get; set; }

        public int Age { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Gender Gender { get; set; }
        public string Phone { get; set; }
    }
}
using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.MongoDatabase;

public class MongoChurchReadModel : Church
{
    [BsonElement("Pastor")]
    public new Pastor Pastor { get; set; }
}
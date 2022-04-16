using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.MongoDatabase.ReadModels;

public class MongoChurchReadModel : Church
{
    [BsonElement("Pastor")]
    public new Pastor Pastor { get; set; }
}
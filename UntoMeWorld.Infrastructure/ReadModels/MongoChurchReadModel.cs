using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Infrastructure.ReadModels;

public class MongoChurchReadModel : Church
{
    [BsonElement("Pastor")]
    public new Pastor? Pastor { get; set; }
}
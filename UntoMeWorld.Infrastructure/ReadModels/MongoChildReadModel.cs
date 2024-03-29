﻿using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Infrastructure.ReadModels;

public class MongoChildReadModel : Child
{
    [BsonElement("Church")]
    public new Church? Church { get; set; }
}
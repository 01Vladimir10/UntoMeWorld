using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.Domain.Model
{
    public class Role : IModel
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public List<Permission> Permissions { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.Domain.Model
{
    public class AppUser: IModel, IRecyclableModel
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public List<string> RoleIds { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
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
        public string Id { get; set; }
        
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AuthProvider { get; set; }
        public string AuthProviderUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public List<string> Roles { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
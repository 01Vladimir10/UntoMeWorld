using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.Domain.Model;

public enum ActionLogType
{
    Delete,
    Add,
    Get,
    Update,
    Purge,
    Restore,
    BulkAdd,
    BulkDelete,
    BulkRestore,
    BulkUpdate,
    BulkPurge,
    Query
}

public enum ActionLogLevel
{
    Debug,
    Info,
    Error,
    Warning
}

public class ActionLog : IModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }

    [BsonRepresentation(BsonType.String)] public ActionLogType Action { get; set; } = ActionLogType.Query;
    [BsonRepresentation(BsonType.String)] public ActionLogLevel Level { get; set; } = ActionLogLevel.Debug;
    
    public string ObjectId { get; set; }
    public string ObjectStore { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    public string IpAddress { get; set; }
    public object Details { get; set; }
    public string AuthMethod { get; set; }
}
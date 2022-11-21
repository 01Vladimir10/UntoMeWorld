using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Query;

namespace UntoMeWorld.Domain.Model;

public class LabelReport : IModel, IRecyclableModel
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public string Name { get; set; }
    public string Template { get; set; }
    public string StyleSheet { get; set; }
    public string Collection { get; set; }
    public QueryFilter Query { get; set; }
    public string OrderBy { get; set; } = nameof(Name);
    public bool OrderDesc { get; set; }
    public int Skip { get; set; } = -1;
    public int Take { get; set; } = -1;
    public bool IsDeleted { get; set; }
    public DateTime DeletedOn { get; set; }
}
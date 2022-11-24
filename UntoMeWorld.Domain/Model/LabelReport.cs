using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Query;
using static System.Double;

namespace UntoMeWorld.Domain.Model;

public class LabelReport : IModel, IRecyclableModel
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    [Required, MaxLength(100)] public string Name { get; set; }
    [Required] public string Template { get; set; }
    [Required] public string StyleSheet { get; set; }
    [Required] public string Collection { get; set; }
    public QueryFilter Query { get; set; }
    [Required] public string OrderBy { get; set; } = nameof(IModel.Id);
    public bool OrderDesc { get; set; }
    [Range(-1, int.MaxValue)] public int Skip { get; set; } = -1;
    [Range(-1, int.MaxValue)] public int Take { get; set; } = -1;
    public bool IsDeleted { get; set; }
    public DateTime DeletedOn { get; set; }
}
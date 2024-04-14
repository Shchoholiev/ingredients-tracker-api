using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using IngredientsTrackerApi.Domain.Common;
using IngredientsTrackerApi.Domain.Enums;

namespace IngredientsTrackerApi.Domain.Entities;

public class Device : EntityBase
{
    public string? Name { get; set; }

    [BsonRepresentation(BsonType.String)]
    public DeviceType Type { get; set; }

    public Guid Guid { get; set; }

    public ObjectId GroupId { get; set; }

    public bool IsActive { get; set; }
}

using IngredientsTrackerApi.Domain.Common;
using IngredientsTrackerApi.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IngredientsTrackerApi.Domain.Entities;

public class Image : EntityBase
{
    public Guid OriginalPhotoGuid { get; set; }

    public Guid SmallPhotoGuid { get; set; }

    public string Extension { get; set; }

    public string Md5Hash { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public ImageUploadState ImageUploadState { get; set; }
}

using IngredientsTrackerApi.Domain.Enums;

namespace IngredientsTrackerApi.Application.Models.Dto;

public class ImageDto
{
    public string Id { get; set; }

    public string OriginalPhotoGuid { get; set; }

    public string SmallPhotoGuid { get; set; }

    public string Extension { get; set; }

    public string Md5Hash { get; set; }

    public ImageUploadState ImageUploadState { get; set; }
}
using AutoMapper;
using Microsoft.Extensions.Logging;
using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Infrastructure.Services.Identity;
using MongoDB.Bson;
using IngredientsTrackerApi.Domain.Enums;
using IngredientsTrackerApi.Application.Models.GlobalInstances;
using System.Security.Cryptography;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using Image = IngredientsTrackerApi.Domain.Entities.Image;
using IngredientsTrackerApi.Application.Exceptions;

namespace IngredientsTrackerApi.Infrastructure.Services;

public class ImagesService(
    IImagesRepository imagesRepository,
    IRecipesRepository recipesRepository,
    ICloudStorageService cloudStorageService,
    ILogger<ImagesService> logger,
    IMapper mapper) : ServiceBase, IImagesService
{
    private readonly IImagesRepository _imagesRepository = imagesRepository;

    private readonly IRecipesRepository _recipesRepository = recipesRepository;

    private readonly ICloudStorageService _cloudStorageService = cloudStorageService;

    private readonly ILogger _logger = logger;

    private readonly IMapper _mapper = mapper;

    public async Task AddRecipeImageAsync(byte[] image, string imageExtension, ObjectId recipeId, CancellationToken cancellationToken)
    {
        var recipe = await _recipesRepository.GetOneAsync(recipeId, cancellationToken);
        if (recipe == null)
        {
            throw new EntityNotFoundException($"Recipe with id {recipeId} not found in a database.");
        }

        var md5Hash = this.GetMd5Hash(image);
        var imageFromDb = await _imagesRepository.GetOneAsync(i => i.Md5Hash == md5Hash, cancellationToken);
        if (imageFromDb != null) {
            recipe.Thumbnail = imageFromDb;
            await _recipesRepository.UpdateRecipeThumbnailAsync(recipeId, recipe, cancellationToken);
            return;
        }
        
        var imageModel = new Image
        {
            OriginalPhotoGuid = Guid.NewGuid(),
            SmallPhotoGuid = Guid.NewGuid(),
            Extension = imageExtension,
            Md5Hash = md5Hash,
            ImageUploadState = ImageUploadState.Started,
            CreatedById = GlobalUser.Id ?? ObjectId.Empty,
            CreatedDateUtc = DateTime.UtcNow
        };
        await _imagesRepository.AddAsync(imageModel, cancellationToken);
        recipe.Thumbnail = imageModel;
        
        await _recipesRepository.UpdateRecipeThumbnailAsync(recipeId, recipe, cancellationToken);

        try
        {
            await Task.WhenAll(
                _cloudStorageService.UploadFileAsync(image, imageModel.OriginalPhotoGuid, imageModel.Extension, cancellationToken),
                ResizeAndUploadImageAsync(imageModel.SmallPhotoGuid, 600, image, imageModel, cancellationToken)
            );
            
            imageModel.ImageUploadState = ImageUploadState.Uploaded;
        }
        catch (Exception)
        {
            imageModel.ImageUploadState = ImageUploadState.Failed;
            throw;
        }
        
        await _imagesRepository.UpdateAsync(imageModel, cancellationToken);
        await _recipesRepository.UpdateRecipeThumbnailAsync(recipeId, recipe, cancellationToken);
    }

    private async Task ResizeAndUploadImageAsync(Guid guid, int width, byte[] image, Image imageModel, CancellationToken cancellationToken)
    {
        var resizedImage = this.ResizeImage(image, width);
        await _cloudStorageService.UploadFileAsync(resizedImage, guid, imageModel.Extension, cancellationToken);
    }

    private byte[] ResizeImage(byte[] imageBytes, int width)
    {
        using var inputStream = new MemoryStream(imageBytes);
        using var outputStream = new MemoryStream();
        using (var image = SixLabors.ImageSharp.Image.Load(inputStream))
        {
            var height = (int)(image.Height * ((float)width / image.Width));

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(width, height),
                Mode = ResizeMode.Max
            }));

            image.Save(outputStream, image.Metadata.DecodedImageFormat);
        }

        return outputStream.ToArray();
    }

    private string GetMd5Hash(byte[] image)
    {
        var hash = MD5.HashData(image);
        var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();
        return hashString;
    }
}

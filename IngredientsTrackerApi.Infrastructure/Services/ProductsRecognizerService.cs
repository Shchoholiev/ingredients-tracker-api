using IngredientsTrackerApi.Application.Exceptions;
using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace IngredientsTrackerApi.Infrastructure.Services;

public class ProductsRecognizerService(
    IDevicesRepository devicesRepository,
    IProductsRepository productsRepository,
    IImageRecognitionService imageRecognitionService,
    ILogger<ProductsRecognizerService> logger) 
    : IProductsRecognizerService
{
    private readonly IDevicesRepository _devicesRepository = devicesRepository;

    private readonly IProductsRepository _productsRepository = productsRepository;
    
    private readonly IImageRecognitionService _imageRecognitionService = imageRecognitionService;

    private readonly ILogger _logger = logger;

    /// <summary>
    /// Recognizes products from an image asynchronously.
    /// </summary>
    /// <param name="deviceGuid">The GUID of the device.</param>
    /// <param name="image">The image to recognize products from.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RecognizeProductsAsync(string deviceGuid, byte[] image, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Recognizing products.");

        var parsedDeviceGuid = Guid.Parse(deviceGuid);
        var productsRecognizer = await _devicesRepository.GetOneAsync(
            d => d.Guid == parsedDeviceGuid && !d.IsDeleted, cancellationToken);
        if (productsRecognizer == null)
        {
            throw new EntityNotFoundException($"Products Recognizer wit Guid: {deviceGuid} is not found in database.");
        }

        var tags = await _imageRecognitionService.GetImageTagsAsync(image, cancellationToken);
        var products = await _productsRepository.GetPageAsync(
            1, 10, 
            p => !p.IsDeleted 
            && p.GroupId == productsRecognizer.GroupId
            && tags.Any(t => t.Name.ToLower() == p.Name.ToLower()), 
            cancellationToken);
        
        if (products.Count > 0)
        {
            var currentProduct = products.First(); 
            currentProduct.Count++;
            await _productsRepository.UpdateAsync(currentProduct, cancellationToken);

            return;
        }

        var product = new Product
        {
            Name = tags.First().Name,
            GroupId = productsRecognizer.GroupId,
            Count = 1,
            CreatedDateUtc = DateTime.UtcNow,
            CreatedById = ObjectId.Empty
        };

        await _productsRepository.AddAsync(product, cancellationToken);
    }
}

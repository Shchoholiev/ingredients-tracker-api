using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Extensions.Logging;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Domain.Common;

namespace IngredientsTrackerApi.Infrastructure.Services;

public class ImageRecognitionService(
    IComputerVisionClient computerVisionClient,
    ILogger<ImageRecognitionService> logger) : IImageRecognitionService
{
    private readonly IComputerVisionClient _computerVisionClient = computerVisionClient;

    private readonly ILogger<ImageRecognitionService> _logger = logger;

    /// <summary>
    /// Retrieves the tags associated with an image using the Computer Vision service.
    /// </summary>
    /// <param name="image">The image data as a byte array.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of tags associated with the image.</returns>
    public async Task<IList<Tag>> GetImageTagsAsync(byte[] image, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting image tags.");
        
        using var imageStream = new MemoryStream(image);

        var tagResult = await _computerVisionClient.TagImageInStreamAsync(imageStream, cancellationToken: cancellationToken);

        var tags = tagResult.Tags
            .Select(x => new Tag
            {
                Name = x.Name,
                Confidence = x.Confidence
            })
            .ToList();
        
        _logger.LogInformation($"Found {tags.Count} tags.");
        
        return tags;
    }
}
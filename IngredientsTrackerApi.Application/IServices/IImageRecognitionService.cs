using IngredientsTrackerApi.Domain.Common;

namespace IngredientsTrackerApi.Application.IServices;

public interface IImageRecognitionService
{
    /// <summary>
    /// Extract tags from an image using ML. 
    /// Currently uses Azure Cognitive Services.
    /// </summary>
    Task<IList<Tag>> GetImageTagsAsync(byte[] image, CancellationToken cancellationToken);
}

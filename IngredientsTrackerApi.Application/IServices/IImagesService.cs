using MongoDB.Bson;

namespace IngredientsTrackerApi.Application.IServices;

public interface IImagesService
{
    Task AddRecipeImageAsync(byte[] image, string imageExtension, ObjectId recipeId, CancellationToken cancellationToken);
}
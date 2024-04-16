using IngredientsTrackerApi.Domain.Entities;
using MongoDB.Bson;

namespace IngredientsTrackerApi.Application.IRepositories;

public interface IRecipesRepository : IBaseRepository<Recipe>
{
    Task UpdateRecipeThumbnailAsync(ObjectId recipeId, Recipe recipe, CancellationToken cancellationToken);
}

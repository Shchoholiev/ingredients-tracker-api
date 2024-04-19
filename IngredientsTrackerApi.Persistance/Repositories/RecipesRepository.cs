using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Domain.Entities;
using IngredientsTrackerApi.Persistance.Database;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IngredientsTrackerApi.Persistance.Repositories;

public class RecipesRepository(MongoDbContext db) 
    : BaseRepository<Recipe>(db, "Recipes"), IRecipesRepository
{
    public async Task<Recipe> UpdateRecipeAsync(Recipe recipe, CancellationToken cancellationToken)
    {
        var updateDefinition = Builders<Recipe>.Update
            .Set(r => r.Name, recipe.Name)
            .Set(r => r.Text, recipe.Text)
            .Set(r => r.Ingredients, recipe.Ingredients)
            .Set(r => r.Categories, recipe.Categories)
            .Set(r => r.LastModifiedDateUtc, recipe.LastModifiedDateUtc)
            .Set(r => r.LastModifiedById, recipe.LastModifiedById);

        var options = new FindOneAndUpdateOptions<Recipe>
        {
            ReturnDocument = ReturnDocument.After
        };

        return await _collection.FindOneAndUpdateAsync(
            Builders<Recipe>.Filter.Eq(r => r.Id, recipe.Id),
            updateDefinition,
            options,
            cancellationToken);
    }

    public Task UpdateRecipeThumbnailAsync(ObjectId recipeId, Recipe recipe, CancellationToken cancellationToken)
    {
        var updateDefinition = Builders<Recipe>.Update
            .Set(r => r.Thumbnail, recipe.Thumbnail)
            .Set(r => r.LastModifiedDateUtc, recipe.LastModifiedDateUtc)
            .Set(r => r.LastModifiedById, recipe.LastModifiedById);

        var options = new FindOneAndUpdateOptions<Recipe>
        {
            ReturnDocument = ReturnDocument.After
        };

        return this._collection.FindOneAndUpdateAsync(
            Builders<Recipe>.Filter.Eq(r => r.Id, recipeId), 
            updateDefinition, 
            options, 
            cancellationToken);
    }    
}

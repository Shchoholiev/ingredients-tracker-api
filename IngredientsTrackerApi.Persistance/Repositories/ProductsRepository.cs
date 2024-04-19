using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Domain.Entities;
using IngredientsTrackerApi.Persistance.Database;
using MongoDB.Driver;

namespace IngredientsTrackerApi.Persistance.Repositories;

public class ProductsRepository(MongoDbContext db) 
    : BaseRepository<Product>(db, "Products"), IProductsRepository
{
    public Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        var updateDefinition = Builders<Product>.Update
            .Set(d => d.Count, product.Count)
            .Set(d => d.LastModifiedDateUtc, product.LastModifiedDateUtc)
            .Set(d => d.LastModifiedById, product.LastModifiedById);

        var options = new FindOneAndUpdateOptions<Product>
        {
            ReturnDocument = ReturnDocument.After
        };

        return this._collection.FindOneAndUpdateAsync(
            Builders<Product>.Filter.Eq(d => d.Id, product.Id), 
            updateDefinition, 
            options, 
            cancellationToken);
    }

    public async Task UpdateManyAsync(IEnumerable<Product> products, CancellationToken cancellationToken)
    {
        var updateModels = new List<WriteModel<Product>>();

        foreach (var product in products)
        {
            var updateDefinition = Builders<Product>.Update
                .Set(d => d.Count, product.Count)
                .Set(d => d.LastModifiedDateUtc, product.LastModifiedDateUtc)
                .Set(d => d.LastModifiedById, product.LastModifiedById);

            updateModels.Add(new UpdateOneModel<Product>(
                Builders<Product>.Filter.Eq(d => d.Id, product.Id), 
                updateDefinition));
        }

        await this._collection.BulkWriteAsync(updateModels, cancellationToken: cancellationToken);
    }
}

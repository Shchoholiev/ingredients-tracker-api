using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Domain.Entities;
using IngredientsTrackerApi.Persistance.Database;
using MongoDB.Driver;

namespace IngredientsTrackerApi.Persistance.Repositories;

public class ImagesRepository(MongoDbContext db) 
    : BaseRepository<Image>(db, "Images"), IImagesRepository
{
    public Task<Image> UpdateAsync(Image image, CancellationToken cancellationToken)
    {
        var updateDefinition = Builders<Image>.Update
            .Set(i => i.ImageUploadState, image.ImageUploadState)
            .Set(i => i.LastModifiedDateUtc, image.LastModifiedDateUtc)
            .Set(i => i.LastModifiedById, image.LastModifiedById);

        var options = new FindOneAndUpdateOptions<Image>
        {
            ReturnDocument = ReturnDocument.After
        };

        return this._collection.FindOneAndUpdateAsync(
            Builders<Image>.Filter.Eq(i => i.Id, image.Id), 
            updateDefinition, 
            options, 
            cancellationToken);
    }
}

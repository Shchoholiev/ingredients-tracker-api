using MongoDB.Driver;
using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Domain.Entities;
using IngredientsTrackerApi.Persistance.Database;

namespace IngredientsTrackerApi.Persistance.Repositories;

public class DeviceRepository : BaseRepository<Device>, IDevicesRepository
{
    public DeviceRepository(MongoDbContext db) : base(db, "Devices") { }
    
    public Task<Device> UpdateAsync(Device device, CancellationToken cancellationToken)
    {
        var updateDefinition = Builders<Device>.Update
            .Set(d => d.Name, device.Name)
            .Set(d => d.GroupId, device.GroupId)
            .Set(d => d.IsActive, device.IsActive)
            .Set(d => d.LastModifiedDateUtc, device.LastModifiedDateUtc)
            .Set(d => d.LastModifiedById, device.LastModifiedById);

        var options = new FindOneAndUpdateOptions<Device>
        {
            ReturnDocument = ReturnDocument.After
        };

        return this._collection.FindOneAndUpdateAsync(
            Builders<Device>.Filter.Eq(d => d.Id, device.Id), 
            updateDefinition, 
            options, 
            cancellationToken);
    }
}

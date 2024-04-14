using IngredientsTrackerApi.Domain.Entities;

namespace IngredientsTrackerApi.Application.IRepositories;

public interface IDevicesRepository : IBaseRepository<Device>
{
    Task<Device> UpdateAsync(Device device, CancellationToken cancellationToken);
}

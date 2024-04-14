using IngredientsTrackerApi.Application.Models;
using IngredientsTrackerApi.Application.Models.AdminDto;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.Application.IServices;

public interface IDevicesService
{
    /// <summary>
    /// Creates a new device and registers it in Azure IoT Hub.
    /// Only a user with <b>Admin</b> role can create a device.
    /// </summary>
    Task<DeviceAdminDto> CreateDeviceAsync(DeviceCreateDto deviceCreateDto, CancellationToken cancellationToken);

    /// <summary>
    /// Returns a device. 
    /// Only a user with <b>Owner</b> role has access.
    /// </summary>
    Task<DeviceDto> GetDeviceAsync(string deviceId, CancellationToken cancellationToken);

    /// <summary>
    /// Returns a page of devices in the group. 
    /// Only a user with <b>Owner</b> role has access.
    /// </summary>
    Task<PagedList<DeviceDto>> GetDevicesPageAsync(int page, int size, string groupdId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates device <c>isActive</c> flag.
    /// Can be used to activate or deactivate a device.
    /// Only a user with <b>Owner</b> role has access.
    /// </summary>
    Task<DeviceDto> UpdateDeviceStatusAsync(string deviceId, DeviceStatusChangeDto deviceDto, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a device. 
    /// Only a user with <b>Owner</b> role has access.
    /// </summary>
    Task<DeviceDto> UpdateDeviceAsync(string deviceId, DeviceUpdateDto deviceUpdateDto, CancellationToken cancellationToken);
}

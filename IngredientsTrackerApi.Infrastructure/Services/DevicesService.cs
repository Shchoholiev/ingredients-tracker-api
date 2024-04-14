using AutoMapper;
using Microsoft.Extensions.Logging;
using IngredientsTrackerApi.Application.Exceptions;
using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Application.Models;
using IngredientsTrackerApi.Application.Models.AdminDto;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.GlobalInstances;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Application.Pagination;
using IngredientsTrackerApi.Domain.Enums;
using IngredientsTrackerApi.Infrastructure.Services.Identity;
using DeviceEntity = IngredientsTrackerApi.Domain.Entities.Device;

namespace IngredientsTrackerApi.Infrastructure.Services;

// TODO: Add checks that user is part of the same group as devices?
public class DevicesService(
    IDevicesRepository devicesRepository,
    ILogger<DevicesService> logger,
    IMapper mapper) : ServiceBase, IDevicesService
{
    private readonly IDevicesRepository _devicesRepository = devicesRepository;

    private readonly ILogger _logger = logger;

    private readonly IMapper _mapper = mapper;

    public async Task<DeviceAdminDto> CreateDeviceAsync(DeviceCreateDto deviceCreateDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating a new device with name {deviceCreateDto.Name}.");

        if (deviceCreateDto.Type == DeviceType.Unknown)
        {
            throw new MissingFieldException("Device type is required.");
        }

        var device = _mapper.Map<DeviceEntity>(deviceCreateDto);
        device.Guid = Guid.NewGuid();
        device.CreatedById = GlobalUser.Id.Value;
        device.CreatedDateUtc = DateTime.UtcNow;

        var createdDevice = await _devicesRepository.AddAsync(device, cancellationToken);
        var deviceDto = _mapper.Map<DeviceAdminDto>(createdDevice);

        _logger.LogInformation($"Device with Id {deviceDto.Id} is created.");

        return deviceDto;
    }

    public async Task<DeviceDto> GetDeviceAsync(string deviceId, CancellationToken cancellationToken)
    {
        var id = ParseObjectId(deviceId);
        var device = await _devicesRepository.GetOneAsync(id, cancellationToken);
        if (device == null)
        {
            throw new EntityNotFoundException($"Device with Id {deviceId} is not found in database.");
        }

        var deviceDto = _mapper.Map<DeviceDto>(device);

        return deviceDto;
    }

    public async Task<PagedList<DeviceDto>> GetDevicesPageAsync(int page, int size, string groupId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Getting a page of devices for group with Id {groupId}.");

        var groupObjectId = ParseObjectId(groupId);
        var devicesTask = _devicesRepository.GetPageAsync(page, size, d => d.GroupId == groupObjectId, cancellationToken);
        var totalCountTask = _devicesRepository.GetCountAsync(d => d.GroupId == groupObjectId, cancellationToken);

        await Task.WhenAll(devicesTask, totalCountTask);

        var devices = await devicesTask;
        var totalCount = await totalCountTask;

        var deviceDtos = _mapper.Map<List<DeviceDto>>(devices);
        var pagedList = new PagedList<DeviceDto>(deviceDtos, page, size, totalCount);

        _logger.LogInformation($"Found {deviceDtos.Count} devices.");

        return pagedList;
    }

    public async Task<DeviceDto> UpdateDeviceAsync(string deviceId, DeviceUpdateDto deviceUpdateDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Updating device with Id {deviceId}.");

        var id = ParseObjectId(deviceId);
        var device = await _devicesRepository.GetOneAsync(id, cancellationToken);
        if (device == null)
        {
            throw new EntityNotFoundException($"Device with Id {deviceId} is not found in database.");
        }

        _mapper.Map(deviceUpdateDto, device);
        device.LastModifiedById = GlobalUser.Id.Value;
        device.LastModifiedDateUtc = DateTime.UtcNow;

        var updatedDevice = await _devicesRepository.UpdateAsync(device, cancellationToken);

        var deviceDto = _mapper.Map<DeviceDto>(updatedDevice);

        _logger.LogInformation($"Device with Id {deviceId} is updated.");

        return deviceDto;
    }

    public async Task<DeviceDto> UpdateDeviceStatusAsync(string deviceId, DeviceStatusChangeDto deviceDto, CancellationToken cancellationToken)
    {
        var id = ParseObjectId(deviceId);
        var device = await _devicesRepository.GetOneAsync(id, cancellationToken);
        if (device == null)
        {
            throw new EntityNotFoundException($"Device with Id {deviceId} is not found in database.");
        }

        if (deviceDto.IsActive)
        {
            device.IsActive = true;
            var groupObjectId = ParseObjectId(deviceDto.GroupId);
            device.GroupId = groupObjectId;
        }
        else
        {
            throw new NotImplementedException("Deactivation of a device is not implemented.");
        }

        device.LastModifiedById = GlobalUser.Id.Value;
        device.LastModifiedDateUtc = DateTime.UtcNow;

        var updatedDevice = await _devicesRepository.UpdateAsync(device, cancellationToken);

        var deviceDtoResult = _mapper.Map<DeviceDto>(updatedDevice);

        _logger.LogInformation($"Device with Id {deviceId} is updated.");

        return deviceDtoResult;
    }
}

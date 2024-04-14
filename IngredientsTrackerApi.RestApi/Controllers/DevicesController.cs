using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Application.Models;
using IngredientsTrackerApi.Application.Models.AdminDto;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.RestApi.Controllers;

[Route("devices")]
public class DevicesController : ApiController
{
    private readonly IDevicesService _devicesService;

    public DevicesController(
        IDevicesService devicesService)
    {
        _devicesService = devicesService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<DeviceAdminDto> CreateDeviceAsync(DeviceCreateDto deviceCreateDto, CancellationToken cancellationToken)
    {
        return await _devicesService.CreateDeviceAsync(deviceCreateDto, cancellationToken);
    }
    
    [HttpGet("{deviceId}")]
    [Authorize(Roles = "Owner")]
    public async Task<DeviceDto> GetDeviceAsync(string deviceId, CancellationToken cancellationToken)
    {
        return await _devicesService.GetDeviceAsync(deviceId, cancellationToken);
    }

    [HttpGet]
    [Authorize(Roles = "Owner")]
    public async Task<PagedList<DeviceDto>> GetDevicesPageAsync(int page, int size, string groupId, CancellationToken cancellationToken)
    {
        return await _devicesService.GetDevicesPageAsync(page, size, groupId, cancellationToken);
    }

    [HttpPatch("{deviceId}/status")]
    [Authorize(Roles = "Owner")]
    public async Task<DeviceDto> UpdateDeviceStatusAsync(string deviceId, DeviceStatusChangeDto deviceDto, CancellationToken cancellationToken)
    {
        return await _devicesService.UpdateDeviceStatusAsync(deviceId, deviceDto, cancellationToken);
    }

    [HttpPut("{deviceId}")]
    [Authorize(Roles = "Owner")]
    public async Task<DeviceDto> UpdateDeviceAsync(string deviceId, DeviceUpdateDto deviceUpdateDto, CancellationToken cancellationToken)
    {
        return await _devicesService.UpdateDeviceAsync(deviceId, deviceUpdateDto, cancellationToken);
    }
}
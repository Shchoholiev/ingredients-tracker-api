using AutoMapper;
using IngredientsTrackerApi.Application.Models;
using IngredientsTrackerApi.Application.Models.AdminDto;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Domain.Entities;

namespace IngredientsTrackerApi.Application.Mapping;

public class DeviceProfile : Profile
{
    public DeviceProfile()
    {
        CreateMap<Device, DeviceDto>();
        CreateMap<Device, DeviceAdminDto>();
        CreateMap<DeviceDto, Device>();
        CreateMap<DeviceCreateDto, Device>();
        CreateMap<DeviceUpdateDto, Device>();
        CreateMap<DeviceUpdateDto, Device>();
    }
}

using AutoMapper;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Domain.Entities;

namespace IngredientsTrackerApi.Application.Mapping;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<Group, GroupDto>();
        CreateMap<GroupDto, Group>();
        CreateMap<GroupCreateDto, Group>();
    }
}
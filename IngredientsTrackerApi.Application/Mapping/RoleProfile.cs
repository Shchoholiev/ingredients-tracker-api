using AutoMapper;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Domain.Entities.Identity;

namespace IngredientsTrackerApi.Application.Mapping;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDto>();
        CreateMap<RoleDto, Role>();
    }
}
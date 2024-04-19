using AutoMapper;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Domain.Entities;

namespace IngredientsTrackerApi.Application.Mapping;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<Recipe, RecipeDto>();
        CreateMap<RecipeDto, Recipe>();
        CreateMap<RecipeCreateDto, Recipe>()
            .ForMember(dest => dest.Thumbnail, opt => opt.Ignore());
        CreateMap<RecipeUpdateDto, Recipe>();
    }
}

using AutoMapper;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Domain.Entities;

namespace IngredientsTrackerApi.Application.Mapping;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<Image, ImageDto>();
        CreateMap<ImageDto, Image>();
    }
}
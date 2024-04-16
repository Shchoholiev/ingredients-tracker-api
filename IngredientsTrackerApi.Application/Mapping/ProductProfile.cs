using AutoMapper;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Domain.Entities;

namespace IngredientsTrackerApi.Application.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
        CreateMap<ProductCreateDto, Product>();
    }
}
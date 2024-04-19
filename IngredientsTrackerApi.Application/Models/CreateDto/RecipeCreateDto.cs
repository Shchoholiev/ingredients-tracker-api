using IngredientsTrackerApi.Application.Models.Dto;
using Microsoft.AspNetCore.Http;

namespace IngredientsTrackerApi.Application.Models.CreateDto;

public class RecipeCreateDto
{   
    public string Name { get; set; }

    public IFormFile? Thumbnail { get; set; }

    public string Text { get; set; }

    public List<ProductDto>? Ingredients { get; set; }

    public List<CategoryDto> Categories { get; set; }

    public string GroupId { get; set; }
}

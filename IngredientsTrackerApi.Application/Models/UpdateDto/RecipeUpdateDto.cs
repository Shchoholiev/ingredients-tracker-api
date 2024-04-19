using IngredientsTrackerApi.Application.Models.Dto;
using Microsoft.AspNetCore.Http;

namespace IngredientsTrackerApi.Application.Models.UpdateDto;

public class RecipeUpdateDto
{
    public string Name { get; set; }

    public IFormFile? Thumbnail { get; set; }

    public string Text { get; set; }

    public List<ProductDto>? Ingredients { get; set; }

    public List<CategoryDto> Categories { get; set; }
}

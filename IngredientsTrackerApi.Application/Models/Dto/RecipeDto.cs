namespace IngredientsTrackerApi.Application.Models.Dto;

public class RecipeDto
{
    public string Id { get; set; }
    
    public string Name { get; set; }

    public ImageDto? Thumbnail { get; set; }

    public string Text { get; set; }

    public List<ProductDto> Ingredients { get; set; }

    public List<CategoryDto> Categories { get; set; }
}

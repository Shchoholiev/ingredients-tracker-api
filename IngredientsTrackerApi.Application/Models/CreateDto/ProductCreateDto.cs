namespace IngredientsTrackerApi.Application.Models.CreateDto;

public class ProductCreateDto
{
    public string Name { get; set; }

    public int Count { get; set; }

    public string GroupId { get; set; }
}

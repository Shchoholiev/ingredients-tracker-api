using IngredientsTrackerApi.Domain.Common;
using MongoDB.Bson;

namespace IngredientsTrackerApi.Domain.Entities;

/// <summary>
/// Food recipe. Contains a list of ingredients and instructions on how to prepare the dish. 
/// </summary>
public class Recipe : EntityBase
{
    public string Name { get; set; }

    public Image? Thumbnail { get; set; }

    public string Text { get; set; }

    public List<Product> Ingredients { get; set; }

    public List<Category> Categories { get; set; }
    
    public ObjectId GroupId { get; set; }
}

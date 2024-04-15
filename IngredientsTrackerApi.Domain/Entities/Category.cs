using IngredientsTrackerApi.Domain.Common;

namespace IngredientsTrackerApi.Domain.Entities;

/// <summary>
/// Recipe category. Can be created by anyone and is shared by all users. Cannot be deleted. <br/>
/// E.g: Breakfast, Lunch, Mexican, Vegan, etc.
/// </summary>
public class Category : EntityBase
{
    public string Name { get; set; }
}

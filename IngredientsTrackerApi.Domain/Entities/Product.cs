using IngredientsTrackerApi.Domain.Common;
using MongoDB.Bson;

namespace IngredientsTrackerApi.Domain.Entities;

/// <summary>
/// Grocery products. Used as ingredients in recipies. <br/>
/// E.g: Eggs, Milk, Flour, etc.
/// </summary>
public class Product : EntityBase
{
    public string Name { get; set; }

    public int Count { get; set; }

    public ObjectId GroupId { get; set; }
}

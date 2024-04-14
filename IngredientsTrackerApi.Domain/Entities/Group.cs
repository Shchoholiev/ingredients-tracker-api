using IngredientsTrackerApi.Domain.Common;

namespace IngredientsTrackerApi.Domain.Entities;

public class Group : EntityBase
{
    public string Name { get; set; }

    public string? Description { get; set; }
}

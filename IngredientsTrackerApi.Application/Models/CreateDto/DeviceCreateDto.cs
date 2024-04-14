using IngredientsTrackerApi.Domain.Enums;

namespace IngredientsTrackerApi.Application.Models.CreateDto;

public class DeviceCreateDto
{
    public string? Name { get; set; }

    public DeviceType Type { get; set; }
}

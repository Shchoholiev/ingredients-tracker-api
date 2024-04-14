using IngredientsTrackerApi.Domain.Enums;

namespace IngredientsTrackerApi.Application.Models.AdminDto;

public class DeviceAdminDto
{
    public string Id { get; set; }

    public string? Name { get; set; }

    public DeviceType Type { get; set; }

    public Guid Guid { get; set; }

    public string AccessKey { get; set; }
}

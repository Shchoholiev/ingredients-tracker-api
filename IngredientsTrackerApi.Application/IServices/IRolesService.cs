using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.Application.IServices;

public interface IRolesService
{
    Task<PagedList<RoleDto>> GetRolesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}

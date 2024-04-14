using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.Application.IServices;

public interface IUsersService
{
    Task<PagedList<UserDto>> GetUsersPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<UserDto> GetUserAsync(string id, CancellationToken cancellationToken);
    
    Task<UserDto> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
}

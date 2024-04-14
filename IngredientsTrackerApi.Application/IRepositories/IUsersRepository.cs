using IngredientsTrackerApi.Domain.Entities.Identity;

namespace IngredientsTrackerApi.Application.IRepositories;

public interface IUsersRepository : IBaseRepository<User>
{
    Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken);
}

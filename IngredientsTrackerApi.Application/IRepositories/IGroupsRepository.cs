using IngredientsTrackerApi.Domain.Entities;

namespace IngredientsTrackerApi.Application.IRepositories;

public interface IGroupsRepository : IBaseRepository<Group>
{
    Task<Group> UpdateAsync(Group group, CancellationToken cancellationToken);
}

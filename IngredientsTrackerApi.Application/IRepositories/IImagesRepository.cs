using IngredientsTrackerApi.Domain.Entities;

namespace IngredientsTrackerApi.Application.IRepositories;

public interface IImagesRepository : IBaseRepository<Image>
{
    Task<Image> UpdateAsync(Image image, CancellationToken cancellationToken);
}

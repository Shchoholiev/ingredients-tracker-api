using IngredientsTrackerApi.Domain.Entities;

namespace IngredientsTrackerApi.Application.IRepositories;

public interface IProductsRepository : IBaseRepository<Product>
{
    Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken);

    Task UpdateManyAsync(IEnumerable<Product> products, CancellationToken cancellationToken);
}

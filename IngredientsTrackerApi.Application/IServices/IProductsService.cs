using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.Application.IServices;

public interface IProductsService
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="productCreateDto">The DTO containing the information of the product to be created.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created product DTO.</returns>
    Task<ProductDto> CreateProductAsync(ProductCreateDto productCreateDto, CancellationToken cancellationToken);

    /// <summary>
    /// Returns a page of products in the group.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The page size.</param>
    /// <param name="groupId">The ID of the group.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged list of product DTOs.</returns>
    Task<PagedList<ProductDto>> GetProductsPageAsync(int page, int size, string groupId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the count of a product.
    /// </summary>
    /// <param name="productId">The ID of the product to update.</param>
    /// <param name="productUpdateDto">The DTO containing the updated count of the product.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated product DTO.</returns>
    Task<ProductDto> UpdateProductCountAsync(string productId, ProductCountUpdateDto productUpdateDto, CancellationToken cancellationToken);
}

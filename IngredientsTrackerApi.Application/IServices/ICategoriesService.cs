using IngredientsTrackerApi.Application.Models;
using IngredientsTrackerApi.Application.Models.AdminDto;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.Application.IServices;

public interface ICategoriesService
{
    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="categoryCreateDto">The DTO containing the data for creating a category.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created category DTO.</returns>
    Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken);

    /// <summary>
    /// Returns a page of categories.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The page size.</param>
    /// <param name="search">The search query by Category name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged list of category DTOs.</returns>
    Task<PagedList<CategoryDto>> GetCategoryPageAsync(int page, int size, string? search, CancellationToken cancellationToken);
}

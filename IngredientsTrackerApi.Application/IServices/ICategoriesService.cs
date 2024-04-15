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
    Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken);

    /// <summary>
    /// Returns a page of categories
    /// </summary>
    Task<PagedList<CategoryDto>> GetCategoryPageAsync(int page, int size, CancellationToken cancellationToken);
}

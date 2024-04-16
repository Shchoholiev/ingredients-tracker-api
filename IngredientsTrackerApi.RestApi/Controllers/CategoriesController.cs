using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.RestApi.Controllers;

[Route("categories")]
public class CategoriesController(ICategoriesService categoriesService) : ApiController
{
    private readonly ICategoriesService _categoriesService = categoriesService;

    [HttpPost]
    [Authorize]
    public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken)
    {
        return await _categoriesService.CreateCategoryAsync(categoryCreateDto, cancellationToken);
    }

    [HttpGet]
    [Authorize]
    public async Task<PagedList<CategoryDto>> GetCategoryPageAsync(int page, int size, string? search, CancellationToken cancellationToken)
    {
        return await _categoriesService.GetCategoryPageAsync(page, size, search, cancellationToken);
    }
}

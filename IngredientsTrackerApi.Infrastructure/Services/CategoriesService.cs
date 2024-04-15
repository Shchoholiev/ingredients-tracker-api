using AutoMapper;
using Microsoft.Extensions.Logging;
using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.GlobalInstances;
using IngredientsTrackerApi.Application.Pagination;
using IngredientsTrackerApi.Infrastructure.Services.Identity;
using IngredientsTrackerApi.Domain.Entities;

namespace IngredientsTrackerApi.Infrastructure.Services;

public class CategoriesService(
    ICategoriesRepository categoriesRepository,
    ILogger<CategoriesService> logger,
    IMapper mapper) : ServiceBase, ICategoriesService
{
    private readonly ICategoriesRepository _categoriesRepository = categoriesRepository;

    private readonly ILogger _logger = logger;

    private readonly IMapper _mapper = mapper;

    public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating a new category with name: {categoryCreateDto.Name}.");

        var category = _mapper.Map<Category>(categoryCreateDto);
        category.CreatedById = GlobalUser.Id.Value;
        category.CreatedDateUtc = DateTime.UtcNow;

        var createdCategory = await _categoriesRepository.AddAsync(category, cancellationToken);
        var categoryDto = _mapper.Map<CategoryDto>(createdCategory);

        _logger.LogInformation($"Category with Id {categoryDto.Id} is created.");

        return categoryDto;
    }

    public async Task<PagedList<CategoryDto>> GetCategoryPageAsync(int page, int size, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Getting a page of categories. Page: {page}, Size: {size}.");

        var categoriesTask = _categoriesRepository.GetPageAsync(page, size, cancellationToken);
        var totalCountTask = _categoriesRepository.GetTotalCountAsync(cancellationToken);

        await Task.WhenAll(categoriesTask, totalCountTask);

        var categories = await categoriesTask;
        var totalCount = await totalCountTask;

        var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
        var pagedList = new PagedList<CategoryDto>(categoryDtos, page, size, totalCount);

        _logger.LogInformation($"Found {categoryDtos.Count} categories.");

        return pagedList;
    }
}

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
using System.Linq.Expressions;
using LinqKit;
using System.Text.RegularExpressions;

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

    public async Task<PagedList<CategoryDto>> GetCategoryPageAsync(int page, int size, string? search, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Getting a page of categories. Page: {page}, Size: {size}.");

        Expression<Func<Category, bool>> predicate = PredicateBuilder.New<Category>(c => !c.IsDeleted);
        if (!string.IsNullOrEmpty(search))
        {
            predicate = predicate.And(c => Regex.IsMatch(c.Name, search, RegexOptions.IgnoreCase));
        }

        var categoriesTask = _categoriesRepository.GetPageAsync(page, size, predicate, cancellationToken);
        var totalCountTask = _categoriesRepository.GetCountAsync(predicate, cancellationToken);

        await Task.WhenAll(categoriesTask, totalCountTask);

        var categories = await categoriesTask;
        var totalCount = await totalCountTask;

        var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
        var pagedList = new PagedList<CategoryDto>(categoryDtos, page, size, totalCount);

        _logger.LogInformation($"Found {categoryDtos.Count} categories.");

        return pagedList;
    }
}

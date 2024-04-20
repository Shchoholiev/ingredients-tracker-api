using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AutoMapper;
using IngredientsTrackerApi.Application.Exceptions;
using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.GlobalInstances;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Application.Pagination;
using IngredientsTrackerApi.Domain.Entities;
using IngredientsTrackerApi.Infrastructure.Services.Identity;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace IngredientsTrackerApi.Infrastructure.Services;

public class RecipesService(
    IRecipesRepository recipesRepository,
    IProductsRepository productsRepository,
    IImagesService imagesService,
    ILogger<RecipesService> logger,
    IMapper mapper) : ServiceBase, IRecipesService
{
    private readonly IRecipesRepository _recipesRepository = recipesRepository;

    private readonly IProductsRepository _productsRepository = productsRepository;

    private readonly IImagesService _imagesService = imagesService;

    private readonly ILogger _logger = logger;

    private readonly IMapper _mapper = mapper;

    public async Task<ProductDto> CreateProductAsync(ProductCreateDto productCreateDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating a new product with name: {productCreateDto.Name}.");

        var product = _mapper.Map<Product>(productCreateDto);
        product.CreatedById = GlobalUser.Id.Value;
        product.CreatedDateUtc = DateTime.UtcNow;

        var createdProduct = await _productsRepository.AddAsync(product, cancellationToken);
        var productDto = _mapper.Map<ProductDto>(createdProduct);

        _logger.LogInformation($"Product with Id {productDto.Id} is created.");

        return productDto;
    }

    public async Task<PagedList<ProductDto>> GetProductsPageAsync(
        int page, int size, string groupId, string? search, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Getting a page of products. Page: {page}, Size: {size}, GroupId: {groupId}.");

        var groupObjectId = ObjectId.Parse(groupId);
        Expression<Func<Product, bool>> predicate = p => p.GroupId == groupObjectId;
        if (!string.IsNullOrEmpty(search))
        {
            predicate = predicate.And(p => Regex.IsMatch(p.Name, search, RegexOptions.IgnoreCase)
                || Regex.IsMatch(p.Description, search, RegexOptions.IgnoreCase));
        }

        var productsTask = _productsRepository.GetPageAsync(page, size, predicate, cancellationToken);
        var totalCountTask = _productsRepository.GetCountAsync(predicate, cancellationToken);

        await Task.WhenAll(productsTask, totalCountTask);

        var products = await productsTask;
        var totalCount = await totalCountTask;

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        var pagedList = new PagedList<ProductDto>(productDtos, page, size, totalCount);

        _logger.LogInformation($"Found {productDtos.Count} products.");

        return pagedList;
    }

    public async Task<ProductDto> UpdateProductCountAsync(string productId, ProductCountUpdateDto productUpdateDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Updating product count for product with Id: {productId}.");

        var productObjectId = ObjectId.Parse(productId);
        var existingProduct = await _productsRepository.GetOneAsync(productObjectId, cancellationToken);

        if (existingProduct == null)
        {
            throw new EntityNotFoundException($"Product with Id {productId} not found.");
        }

        existingProduct.Count = productUpdateDto.Count;

        var updatedProduct = await _productsRepository.UpdateAsync(existingProduct, cancellationToken);
        var productDto = _mapper.Map<ProductDto>(updatedProduct);

        _logger.LogInformation($"Product count updated for product with Id: {productId}.");

        return productDto;
    }

    public async Task<RecipeDto> CreateRecipeAsync(RecipeCreateDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating a new recipe with name: {dto.Name}.");

        var entity = this._mapper.Map<Recipe>(dto);

        entity.CreatedById = GlobalUser.Id.Value;
        entity.CreatedDateUtc = DateTime.UtcNow;

        var recipe = await this._recipesRepository.AddAsync(entity, cancellationToken);
        if (dto.Thumbnail != null)
        {
            _logger.LogInformation($"Adding thumbnail for recipe with Id: {recipe.Id}.");

            using var memoryStream = new MemoryStream();
            await dto.Thumbnail.CopyToAsync(memoryStream, cancellationToken);
            var extension = Path.GetExtension(dto.Thumbnail.FileName).Substring(1).ToLower();
            Task.Run(() => _imagesService.AddRecipeImageAsync(memoryStream.ToArray(), extension, recipe.Id, cancellationToken));
        }

        _logger.LogInformation($"Recipe with Id {recipe.Id} is created.");

        return this._mapper.Map<RecipeDto>(recipe);
    }

    public async Task<RecipeDto> UpdateRecipeAsync(string id, RecipeUpdateDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Updating recipe with Id: {id}.");

        var recipeObjectId = ObjectId.Parse(id);
        var recipe = await _recipesRepository.GetOneAsync(recipeObjectId, cancellationToken);
        if (recipe == null)
        {
            throw new EntityNotFoundException($"Recipe with Id {id} not found.");
        }

        _mapper.Map(dto, recipe);
        recipe.LastModifiedById = GlobalUser.Id.Value;
        recipe.LastModifiedDateUtc = DateTime.UtcNow;

        var updatedRecipe = await _recipesRepository.UpdateRecipeAsync(recipe, cancellationToken);
        if (dto.Thumbnail != null)
        {
            _logger.LogInformation($"Adding thumbnail for recipe with Id: {recipe.Id}.");

            using var memoryStream = new MemoryStream();
            await dto.Thumbnail.CopyToAsync(memoryStream, cancellationToken);
            var extension = Path.GetExtension(dto.Thumbnail.FileName).Substring(1).ToLower();
            Task.Run(() => _imagesService.AddRecipeImageAsync(memoryStream.ToArray(), extension, recipe.Id, cancellationToken));
        }

        var recipeDto = _mapper.Map<RecipeDto>(updatedRecipe);

        _logger.LogInformation($"Recipe with Id: {id} updated.");

        return recipeDto;
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Deleting recipe with Id: {id}.");

        var recipeObjectId = ObjectId.Parse(id);
        var existingRecipe = await _recipesRepository.GetOneAsync(recipeObjectId, cancellationToken);

        if (existingRecipe == null)
        {
            throw new EntityNotFoundException($"Recipe with Id {id} not found.");
        }

        await _recipesRepository.DeleteAsync(existingRecipe, cancellationToken);

        _logger.LogInformation($"Recipe with Id: {id} deleted.");
    }

    public async Task<RecipeDto> GetRecipeAsync(string id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Getting recipe with Id: {id}.");

        var recipeObjectId = ObjectId.Parse(id);
        var recipe = await _recipesRepository.GetOneAsync(recipeObjectId, cancellationToken);

        if (recipe == null)
        {
            throw new EntityNotFoundException($"Recipe with Id {id} not found.");
        }

        var recipeDto = _mapper.Map<RecipeDto>(recipe);

        _logger.LogInformation($"Recipe with Id: {id} found.");

        return recipeDto;
    }

    /// <summary>
    /// Retrieves a page of recipes based on the specified criteria.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of recipes per page.</param>
    /// <param name="groupId">The ID of the group to filter recipes by.</param>
    /// <param name="search">The search string to filter recipes by name (optional).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="PagedList{T}"/> of <see cref="RecipeDto"/>.</returns>
    public async Task<PagedList<RecipeDto>> GetRecipesPageAsync(int pageNumber, int pageSize, string groupId, string? search, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Getting a page of recipes. Page: {pageNumber}, Size: {pageSize}, GroupId: {groupId}.");

        var groupObjectId = ObjectId.Parse(groupId);
        Expression<Func<Recipe, bool>> predicate = r => r.GroupId == groupObjectId;
        if (!string.IsNullOrEmpty(search))
        {
            predicate = predicate.And(r => Regex.IsMatch(r.Name, search, RegexOptions.IgnoreCase));
        }

        var recipesTask = _recipesRepository.GetPageAsync(pageNumber, pageSize, predicate, cancellationToken);
        var totalCountTask = _recipesRepository.GetCountAsync(predicate, cancellationToken);

        await Task.WhenAll(recipesTask, totalCountTask);

        var recipes = await recipesTask;
        var totalCount = await totalCountTask;

        var recipeDtos = _mapper.Map<List<RecipeDto>>(recipes);
        var pagedList = new PagedList<RecipeDto>(recipeDtos, pageNumber, pageSize, totalCount);

        _logger.LogInformation($"Found {recipeDtos.Count} recipes.");

        return pagedList;
    }
    
    public async Task CookRecipeAsync(string id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Cooking recipe with Id: {id}.");

        var recipeObjectId = ObjectId.Parse(id);
        var recipe = await _recipesRepository.GetOneAsync(recipeObjectId, cancellationToken);
        if (recipe == null)
        {
            throw new EntityNotFoundException($"Recipe with Id {id} not found.");
        }

        var products = await _productsRepository.GetPageAsync(
            1, 100,
            p => recipe.Ingredients.Any(i => i.Id == p.Id),
            cancellationToken);

        _logger.LogInformation($"Checking if group has all ingredients for the recipe.");

        foreach (var ingredient in recipe.Ingredients)
        {
            var product = products.FirstOrDefault(p => p.Id == ingredient.Id);
            if (product == null)
            {
                throw new EntityNotFoundException($"Product with Id {ingredient.Id} not found.");
            }

            if (product.Count < ingredient.Count)
            {
                throw new InvalidOperationException($"Insufficient quantity of ingredient \"{product.Name}\".");
            }

            product.Count -= ingredient.Count;
        }

        await _productsRepository.UpdateManyAsync(products, cancellationToken);

        _logger.LogInformation($"Recipe with Id: {id} cooked.");
    }
}

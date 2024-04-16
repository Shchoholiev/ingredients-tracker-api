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
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace IngredientsTrackerApi.Infrastructure.Services;

public class ProductsService(
    IProductsRepository productsRepository,
    ILogger<ProductsService> logger,
    IMapper mapper) : ServiceBase, IProductsService
{
    private readonly IProductsRepository _productsRepository = productsRepository;

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
            predicate = predicate.And(p => Regex.IsMatch(p.Name, search, RegexOptions.IgnoreCase));
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
}

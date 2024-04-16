using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.RestApi.Controllers;

[Route("products")]
public class ProductsController(
    IProductsService productsService) : ApiController
{
    private readonly IProductsService _productsService = productsService;

    [HttpPost]
    [Authorize]
    public async Task<ProductDto> CreateProductAsync(ProductCreateDto productCreateDto, CancellationToken cancellationToken)
    {
        return await _productsService.CreateProductAsync(productCreateDto, cancellationToken);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<PagedList<ProductDto>> GetProductsPageAsync(int page, int size, string groupId, string? search, CancellationToken cancellationToken)
    {
        return await _productsService.GetProductsPageAsync(page, size, groupId, search, cancellationToken);
    }

    [HttpPatch("{productId}/count")]
    [Authorize]
    public async Task<ProductDto> UpdateProductCountAsync(string productId, ProductCountUpdateDto productUpdateDto, CancellationToken cancellationToken)
    {
        return await _productsService.UpdateProductCountAsync(productId, productUpdateDto, cancellationToken);
    }
}
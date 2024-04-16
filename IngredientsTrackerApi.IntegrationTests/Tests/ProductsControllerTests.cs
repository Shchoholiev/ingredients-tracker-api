using System.Net;
using System.Net.Http.Json;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.IntegrationTests.Tests;

public class ProductsControllerTests(TestingFactory<Program> factory) 
    : TestsBase(factory, "products")
{

    #region CreateProductAsync

    [Fact]
    public async Task CreateProductAsync_ValidInput_ReturnsProductDto()
    {
        // Arrange
        await LoginAsync("test@gmail.com", "Yuiop12345");
        var productCreateDto = new ProductCreateDto
        {
            Name = "Eggs",
            Count = 12,
            GroupId = "652c3b89ae02a3135d6429fc",
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync($"{ResourceUrl}", productCreateDto);
        var productDto = await response.Content.ReadFromJsonAsync<ProductDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(productDto);
        Assert.Equal(productCreateDto.Name, productDto.Name);
        Assert.Equal(productCreateDto.Count, productDto.Count);
    }

    [Fact]
    public async Task CreateProductAsync_UnauthorizedUser_ReturnsUnauthorized()
    {
        // Arrange
        var productCreateDto = new ProductCreateDto
        {
            // Set the properties of the productCreateDto object
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync($"{ResourceUrl}", productCreateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion

    #region GetProductsPageAsync

    [Fact]
    public async Task GetProductsPageAsync_ValidInput_ReturnsProductList()
    {
        // Arrange
        await LoginAsync("test@gmail.com", "Yuiop12345");
        var groupId = "652c3b89ae02a3135d6429fc";
        int page = 1;
        int size = 10;

        // Act
        var response = await HttpClient.GetAsync($"{ResourceUrl}?page={page}&size={size}&groupId={groupId}");
        var productDtos = await response.Content.ReadFromJsonAsync<PagedList<ProductDto>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(productDtos);
        Assert.NotEmpty(productDtos.Items);
    }

    [Fact]
    public async Task GetProductsPageAsync_UnauthorizedUser_ReturnsUnauthorized()
    {
        // Arrange
        var groupId = "652c3b89ae02a3135d6429fc";
        int page = 1;
        int size = 10;

        // Act
        var response = await HttpClient.GetAsync($"{ResourceUrl}?page={page}&size={size}&groupId={groupId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion

    #region UpdateProductCountAsync

    [Fact]
    public async Task UpdateProductCountAsync_ValidInput_ReturnsUpdatedProductDto()
    {
        // Arrange
        await LoginAsync("test@gmail.com", "Yuiop12345");
        var productId = "801c3b89ae02a3135d6429fc";
        var productCountUpdateDto = new ProductCountUpdateDto
        {
            Count = 5
        };

        // Act
        var response = await HttpClient.PatchAsJsonAsync($"{ResourceUrl}/{productId}/count", productCountUpdateDto);
        var updatedProductDto = await response.Content.ReadFromJsonAsync<ProductDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(updatedProductDto);
        Assert.Equal(productCountUpdateDto.Count, updatedProductDto.Count);
    }

    [Fact]
    public async Task UpdateProductCountAsync_UnauthorizedUser_ReturnsUnauthorized()
    {
        // Arrange
        var productId = "801c3b89ae02a3135d6429fc";
        var productCountUpdateDto = new ProductCountUpdateDto
        {
            Count = 5
        };

        // Act
        var response = await HttpClient.PatchAsJsonAsync($"{ResourceUrl}/{productId}/count", productCountUpdateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion
}

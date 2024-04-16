using System.Net;
using System.Net.Http.Json;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.IntegrationTests.Tests;

public class CategoriesControllerTests(TestingFactory<Program> factory) 
    : TestsBase(factory, "categories")
{
    #region CreateCategoryAsync

    [Fact]
    public async Task CreateCategoryAsync_ValidInput_ReturnsCategoryDto()
    {
        // Arrange
        await LoginAsync("test@gmail.com", "Yuiop12345");
        var categoryCreateDto = new CategoryCreateDto
        {
            Name = "Integration Test Category"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync($"{ResourceUrl}", categoryCreateDto);
        var categoryDto = await response.Content.ReadFromJsonAsync<CategoryDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(categoryDto);
        Assert.Equal("Integration Test Category", categoryDto.Name);
    }

    [Fact]
    public async Task CreateCategoryAsync_MissingName_ReturnsBadRequest()
    {
        // Arrange
        await LoginAsync("admin@gmail.com", "Yuiop12345");
        var categoryCreateDto = new CategoryCreateDto
        {
            // No name should cause a bad request
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync($"{ResourceUrl}", categoryCreateDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region GetCategoryPageAsync

    [Fact]
    public async Task GetCategoryPageAsync_CategoriesExist_ReturnsCategoryList()
    {
        // Arrange
        await LoginAsync("owner@gmail.com", "Yuiop12345");
        int page = 1;
        int size = 10;

        // Act
        var response = await HttpClient.GetAsync($"{ResourceUrl}?page={page}&size={size}");
        var categoryDtos = await response.Content.ReadFromJsonAsync<PagedList<CategoryDto>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(categoryDtos);
        Assert.NotEmpty(categoryDtos.Items);
    }

    [Fact]
    public async Task GetCategoryPageAsync_SearchByName_ReturnsCategoryList()
    {
        // Arrange
        await LoginAsync("owner@gmail.com", "Yuiop12345");
        int page = 1;
        int size = 10;
        var search = "breakfast";

        // Act
        var response = await HttpClient.GetAsync($"{ResourceUrl}?page={page}&size={size}&search={search}");
        var categoryDtos = await response.Content.ReadFromJsonAsync<PagedList<CategoryDto>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(categoryDtos);
        Assert.NotEmpty(categoryDtos.Items);
        Assert.Single(categoryDtos.Items);
    }

    [Fact]
    public async Task GetCategoryPageAsync_NoCategoriesExist_ReturnsEmptyList()
    {
        // Arrange
        await LoginAsync("owner@gmail.com", "Yuiop12345");
        int page = 3;
        int size = 10;

        // Act
        var response = await HttpClient.GetAsync($"{ResourceUrl}?page={page}&size={size}");
        var categoryDtos = await response.Content.ReadFromJsonAsync<PagedList<CategoryDto>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(categoryDtos);
        Assert.Empty(categoryDtos.Items);
    }

    [Fact]
    public async Task GetCategoryPageAsync_UnauthorizedUser_ReturnsUnauthorized()
    {
        // Arrange
        int page = 1;
        int size = 10;

        // Act
        var response = await HttpClient.GetAsync($"{ResourceUrl}?page={page}&size={size}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion
}

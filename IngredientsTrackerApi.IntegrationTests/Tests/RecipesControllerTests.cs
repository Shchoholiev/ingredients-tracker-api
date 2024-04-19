using System.Net;
using System.Net.Http.Json;
using System.Text;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Application.Pagination;
using Microsoft.AspNetCore.Http;

namespace IngredientsTrackerApi.IntegrationTests.Tests;
public class RecipesControllerTests(TestingFactory<Program> factory)
    : TestsBase(factory, "recipes")
{
    #region CreateRecipeAsync

    [Fact]
    public async Task CreateRecipeAsync_ValidInput_ReturnsRecipeDto()
    {
        // Arrange
        await LoginAsync("test@gmail.com", "Yuiop12345");

        var projectDir = Environment.CurrentDirectory;
        var imagePath = Path.Combine(projectDir, "Media", "borscht.png");

        var multipartContent = new MultipartFormDataContent
        {
            { new StringContent("Borscht"), "Name" },
            { new StringContent("Sauté onions, add grated beets, carrots, and broth. Simmer with cabbage and potatoes until tender. Season with salt, pepper, and dill."), "Text" },
            { new ByteArrayContent(File.ReadAllBytes(imagePath)), "Thumbnail", "borscht.png" }
        };

        var ingredients = new[]
        {
            new KeyValuePair<string, string>("Ingredients[0].Id", "803c3b89ae02a3135d6429fc"),
            new KeyValuePair<string, string>("Ingredients[0].Name", "Beetroot"),
            new KeyValuePair<string, string>("Ingredients[0].Count", "2"),
            new KeyValuePair<string, string>("Ingredients[1].Id", "804c3b89ae02a3135d6429fc"),
            new KeyValuePair<string, string>("Ingredients[1].Name", "Cabbage"),
            new KeyValuePair<string, string>("Ingredients[1].Count", "1"),
            new KeyValuePair<string, string>("Ingredients[2].Id", "805c3b89ae02a3135d6429fc"),
            new KeyValuePair<string, string>("Ingredients[2].Name", "Potatoes"),
            new KeyValuePair<string, string>("Ingredients[2].Count", "2"),
            new KeyValuePair<string, string>("Ingredients[3].Id", "806c3b89ae02a3135d6429fc"),
            new KeyValuePair<string, string>("Ingredients[3].Name", "Carrots"),
            new KeyValuePair<string, string>("Ingredients[3].Count", "2"),
            new KeyValuePair<string, string>("Ingredients[4].Id", "807c3b89ae02a3135d6429fc"),
            new KeyValuePair<string, string>("Ingredients[4].Name", "Onion"),
            new KeyValuePair<string, string>("Ingredients[4].Count", "1"),
            new KeyValuePair<string, string>("Ingredients[5].Id", "808c3b89ae02a3135d6429fc"),
            new KeyValuePair<string, string>("Ingredients[5].Name", "Tomato Paste"),
            new KeyValuePair<string, string>("Ingredients[5].Count", "2"),
        };
        foreach (var ingredient in ingredients)
        {
            multipartContent.Add(new StringContent(ingredient.Value), ingredient.Key);
        }

        multipartContent.Add(new StringContent("762c3b89ae02a3135d6429fc"), "Categories[0].Id");
        multipartContent.Add(new StringContent("Ukrainian"), "Categories[0].Name");
        multipartContent.Add(new StringContent("652c3b89ae02a3135d6429fc"), "GroupId");

        // Act
        var response = await HttpClient.PostAsync($"{ResourceUrl}", multipartContent);
        var a = await response.Content.ReadAsStringAsync();
        var recipeDto = await response.Content.ReadFromJsonAsync<RecipeDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(recipeDto);
        Assert.Equal("Borscht", recipeDto.Name);
        Assert.Equal(6, recipeDto.Ingredients.Count);
        Assert.Single(recipeDto.Categories);
    }

    [Fact]
    public async Task CreateRecipeAsync_UnauthorizedUser_ReturnsUnauthorized()
    {
        // Arrange
        var recipeCreateDto = new RecipeCreateDto
        {
            Name = "Test",
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync($"{ResourceUrl}", recipeCreateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    #endregion

    #region UpdateRecipeAsync

    [Fact]
    public async Task UpdateRecipeAsync_ValidInput_ReturnsRecipeDto()
    {
        // Arrange
        await LoginAsync("test@gmail.com", "Yuiop12345");
        var recipeId = "901c3b89ae02a3135d6429fc";

        var multipartContent = new MultipartFormDataContent
        {
            { new StringContent("Cabbage Salad"), "Name" },
            { new StringContent("Updated"), "Text" },
        };

        var ingredients = new[]
        {
            new KeyValuePair<string, string>("Ingredients[0].Id", "804c3b89ae02a3135d6429fc"),
            new KeyValuePair<string, string>("Ingredients[0].Name", "Cabbage"),
            new KeyValuePair<string, string>("Ingredients[0].Count", "1"),
            new KeyValuePair<string, string>("Ingredients[1].Id", "807c3b89ae02a3135d6429fc"),
            new KeyValuePair<string, string>("Ingredients[1].Name", "Onion"),
            new KeyValuePair<string, string>("Ingredients[1].Count", "2"),
        };
        foreach (var ingredient in ingredients)
        {
            multipartContent.Add(new StringContent(ingredient.Value), ingredient.Key);
        }

        multipartContent.Add(new StringContent("752c3b89ae02a3135d6429fc"), "Categories[0].Id");
        multipartContent.Add(new StringContent("Breakfast"), "Categories[0].Name");
        multipartContent.Add(new StringContent("652c3b89ae02a3135d6429fc"), "GroupId");

        multipartContent.Add(new StringContent("762c3b89ae02a3135d6429fc"), "Categories[1].Id");
        multipartContent.Add(new StringContent("Ukrainian"), "Categories[1].Name");
        multipartContent.Add(new StringContent("652c3b89ae02a3135d6429fc"), "GroupId");

        // Act
        var response = await HttpClient.PutAsync($"{ResourceUrl}/{recipeId}", multipartContent);
        var recipeDto = await response.Content.ReadFromJsonAsync<RecipeDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(recipeDto);
        Assert.Equal("Cabbage Salad", recipeDto.Name);
        Assert.Equal("Updated", recipeDto.Text);
        Assert.Equal(2, recipeDto.Categories.Count);
    }

    #endregion

    [Fact]
    public async Task DeleteRecipeAsync_ValidInput_ReturnsNoContent()
    {
        // Arrange
        await LoginAsync("test@gmail.com", "Yuiop12345");
        var recipeId = "903c3b89ae02a3135d6429fc";

        // Act
        var response = await HttpClient.DeleteAsync($"{ResourceUrl}/{recipeId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetRecipeAsync_ValidInput_ReturnsRecipeDto()
    {
        // Arrange
        await LoginAsync("test@gmail.com", "Yuiop12345");
        var recipeId = "901c3b89ae02a3135d6429fc";

        // Act
        var response = await HttpClient.GetAsync($"{ResourceUrl}/{recipeId}");
        var recipeDto = await response.Content.ReadFromJsonAsync<RecipeDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(recipeDto);
        Assert.Equal("Cabbage Salad", recipeDto.Name);
        Assert.Equal(2, recipeDto.Ingredients.Count);
    }

    #region GetRecipesPageAsync

    [Fact]
    public async Task GetRecipesPageAsync_ValidInput_ReturnsPagedListRecipeDto()
    {
        // Arrange
        await LoginAsync("test@gmail.com", "Yuiop12345");
        int page = 1;
        int size = 10;
        string groupId = "652c3b89ae02a3135d6429fc";
        string search = "search";

        // Act
        var response = await HttpClient.GetAsync($"{ResourceUrl}?page={page}&size={size}&groupId={groupId}&search={search}");
        var pagedRecipeDto = await response.Content.ReadFromJsonAsync<PagedList<RecipeDto>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(pagedRecipeDto);
    }

    [Fact]
    public async Task GetRecipesPageAsync_WithSearch_ReturnsPagedListWithSingleRecipe()
    {
        // Arrange
        await LoginAsync("test@gmail.com", "Yuiop12345");
        int page = 1;
        int size = 10;
        string groupId = "652c3b89ae02a3135d6429fc";
        string search = "Fried";

        // Act
        var response = await HttpClient.GetAsync($"{ResourceUrl}?page={page}&size={size}&groupId={groupId}&search={search}");
        var pagedRecipeDto = await response.Content.ReadFromJsonAsync<PagedList<RecipeDto>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(pagedRecipeDto);
        Assert.Single(pagedRecipeDto.Items);
    }

    #endregion

    #region CookRecipeAsync

    [Fact]
    public async Task CookRecipeAsync_SufficientIngredients_ReturnsOK()
    {
        // Arrange
        await LoginAsync("test@gmail.com", "Yuiop12345");
        var recipeId = "902c3b89ae02a3135d6429fc";

        // Act
        var response = await HttpClient.PatchAsync($"{ResourceUrl}/{recipeId}/cook", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CookRecipeAsync_InsufficientIngredients_ReturnsConflict()
    {
        // Arrange
        await LoginAsync("test@gmail.com", "Yuiop12345");
        var recipeId = "901c3b89ae02a3135d6429fc";

        // Act
        var response = await HttpClient.PatchAsync($"{ResourceUrl}/{recipeId}/cook", null);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    #endregion
}

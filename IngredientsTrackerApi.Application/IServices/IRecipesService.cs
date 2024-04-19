using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.Application.IServices;

public interface IRecipesService
{
    /// <summary>
    /// Adds a new recipe asynchronously.
    /// </summary>
    /// <param name="dto">The recipe data transfer object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The added recipe as a data transfer object.</returns>
    Task<RecipeDto> CreateRecipeAsync(RecipeCreateDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing recipe asynchronously.
    /// </summary>
    /// <param name="id">The ID of the recipe to update.</param>
    /// <param name="dto">The updated recipe data transfer object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated recipe as a data transfer object.</returns>
    Task<RecipeDto> UpdateRecipeAsync(string id, RecipeUpdateDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a recipe asynchronously.
    /// </summary>
    /// <param name="id">The ID of the recipe to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task DeleteAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a recipe asynchronously by its ID.
    /// </summary>
    /// <param name="id">The ID of the recipe to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The retrieved recipe as a data transfer object.</returns>
    Task<RecipeDto> GetRecipeAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a page of recipes asynchronously.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of recipes per page.</param>
    /// <param name="groupId">The ID of the group to filter by.</param>
    /// <param name="search">The search query to filter by (optional).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged list of recipes as data transfer objects.</returns>
    Task<PagedList<RecipeDto>> GetRecipesPageAsync(
        int pageNumber, int pageSize, string groupId, string? search, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if group has all ingredients for the recipe. If it does then it decreases ingredients count.
    /// </summary>
    /// <param name="id">The ID of the recipe to cook.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task CookRecipeAsync(string id, CancellationToken cancellationToken);
}

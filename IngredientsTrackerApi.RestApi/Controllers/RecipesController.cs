using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Application.Models.CreateDto;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.UpdateDto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.RestApi.Controllers;

[Route("recipes")]
public class RecipesController(IRecipesService recipesService) : ApiController
{
    private readonly IRecipesService _recipesService = recipesService;

    [HttpPost]
    [Authorize]
    public async Task<RecipeDto> CreateRecipeAsync(RecipeCreateDto recipeCreateDto, CancellationToken cancellationToken)
    {
        return await _recipesService.CreateRecipeAsync(recipeCreateDto, cancellationToken);
    }
    
    [HttpPut("{recipeId}")]
    [Authorize]
    public async Task<RecipeDto> UpdateRecipeAsync(string recipeId, RecipeUpdateDto recipeUpdateDto, CancellationToken cancellationToken)
    {
        return await _recipesService.UpdateRecipeAsync(recipeId, recipeUpdateDto, cancellationToken);
    }

    [HttpDelete("{recipeId}")]
    [Authorize]
    public async Task<IActionResult> DeleteRecipeAsync(string recipeId, CancellationToken cancellationToken)
    {
        await _recipesService.DeleteAsync(recipeId, cancellationToken);
        return NoContent();
    }

    [HttpGet("{recipeId}")]
    [Authorize]
    public async Task<RecipeDto> GetRecipeAsync(string recipeId, CancellationToken cancellationToken)
    {
        return await _recipesService.GetRecipeAsync(recipeId, cancellationToken);
    }

    [HttpGet]
    [Authorize]
    public async Task<PagedList<RecipeDto>> GetRecipesPageAsync(
        int page, int size, string groupId, string? search, CancellationToken cancellationToken)
    {
        return await _recipesService.GetRecipesPageAsync(page, size, groupId, search, cancellationToken);
    }

    [HttpPatch("{recipeId}/cook")]
    [Authorize]
    public async Task<IActionResult> CookRecipeAsync(string recipeId, CancellationToken cancellationToken)
    {
        await _recipesService.CookRecipeAsync(recipeId, cancellationToken);
        return Ok();
    }
}
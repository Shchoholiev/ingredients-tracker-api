using Microsoft.AspNetCore.Mvc;
using IngredientsTrackerApi.Application.IServices.Identity;
using IngredientsTrackerApi.Application.Models.Identity;

namespace IngredientsTrackerApi.RestApi.Controllers;

[Route("tokens")]
public class TokensController(IUserManager userManager) : ApiController
{
    private readonly IUserManager _userManager = userManager;

    [HttpPost("refresh")]
    public async Task<ActionResult<TokensModel>> RefreshAccessTokenAsync([FromBody] TokensModel tokensModel, CancellationToken cancellationToken)
    {
        var refreshedTokens = await _userManager.RefreshAccessTokenAsync(tokensModel, cancellationToken);
        return Ok(refreshedTokens);
    }
}

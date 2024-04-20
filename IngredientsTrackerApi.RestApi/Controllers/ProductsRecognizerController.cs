using Microsoft.AspNetCore.Mvc;
using IngredientsTrackerApi.Application.IServices;

namespace IngredientsTrackerApi.RestApi.Controllers;

[Route("products-recognizers")]
public class ProductsRecognizerController(
    IProductsRecognizerService productsRecognizerService
) : ApiController
{
    private readonly IProductsRecognizerService _productsRecognizerService = productsRecognizerService;

    [HttpPost("{deviceGuid}/products/identify-by-image")]
    public async Task<ActionResult> RecognizeProductsAsync(string deviceGuid, IFormFile image, CancellationToken cancellationToken)
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest("Image file is required.");
        }

        byte[] imageData;
        using (var memoryStream = new MemoryStream())
        {
            await image.CopyToAsync(memoryStream, cancellationToken);
            imageData = memoryStream.ToArray();
        }

        await _productsRecognizerService.RecognizeProductsAsync(deviceGuid, imageData, cancellationToken);

        return Ok();
    }
}

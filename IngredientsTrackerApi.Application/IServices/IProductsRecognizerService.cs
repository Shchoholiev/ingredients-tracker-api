namespace IngredientsTrackerApi.Application.IServices;

public interface IProductsRecognizerService
{
    Task RecognizeProductsAsync(string deviceGuid, byte[] image, CancellationToken cancellationToken);
}

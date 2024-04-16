namespace IngredientsTrackerApi.Application.IServices;

public interface ICloudStorageService
{
    /// <summary>
    /// Uploads a file to the cloud storage asynchronously.
    /// </summary>
    /// <param name="file">The byte array representing the file to upload.</param>
    /// <param name="guid">The unique identifier for the file.</param>
    /// <param name="fileExtension">The file extension of the file.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the URL of the uploaded file.</returns>
    Task<string> UploadFileAsync(byte[] file, Guid guid, string fileExtension, CancellationToken cancellationToken);
}

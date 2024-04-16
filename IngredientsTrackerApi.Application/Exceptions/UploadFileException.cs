namespace IngredientsTrackerApi.Application.Exceptions;

public class UploadFileException : Exception
{
    public UploadFileException() { }

	public UploadFileException(string fileName, string bucketName) 
        : base(string.Format($"Could not upload {fileName} to {bucketName}."))
    { }
}

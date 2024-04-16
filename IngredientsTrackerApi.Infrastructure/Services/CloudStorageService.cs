using Amazon.S3;
using Amazon.S3.Model;
using IngredientsTrackerApi.Application.Exceptions;
using IngredientsTrackerApi.Application.IServices;
using Microsoft.Extensions.Configuration;

namespace IngredientsTrackerApi.Infrastructure.Services;

public class CloudStorageService : ICloudStorageService
{
    private readonly string _bucketName;
    private readonly string _objectUrl;
    private readonly AmazonS3Client _s3Client;

    public CloudStorageService(IConfiguration configuration)
    {
        this._bucketName = configuration.GetSection("CloudObjectStorage")["BucketName"];
        var config = new AmazonS3Config
        {
            ServiceURL = configuration.GetConnectionString("StorageEndpoint")
        };
        var accessKey = configuration.GetSection("CloudObjectStorage")["AccessKey"];
        var secretKey = configuration.GetSection("CloudObjectStorage")["SecretKey"];
        this._objectUrl = configuration.GetSection("CloudObjectStorage")["ObjectUrl"];
        this._s3Client = new AmazonS3Client(accessKey, secretKey, config);
    }

    public async Task<string> UploadFileAsync(byte[] file, Guid guid, string fileExtension, CancellationToken cancellationToken)
    {
        var fileName = guid.ToString()+ "." + fileExtension;
        using var newMemoryStream = new MemoryStream(file);
        var request = new PutObjectRequest()
        {
            BucketName = _bucketName,
            Key = fileName,
            InputStream = newMemoryStream
        };

        var response = await _s3Client.PutObjectAsync(request);
        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine($"Successfully uploaded {fileName} to {this._bucketName}.");

            var fileUrl = this._objectUrl + this._bucketName + "/" + fileName;
            return fileUrl;
        }
        else
        {
            throw new UploadFileException(fileName, this._bucketName);
        }
    }
}
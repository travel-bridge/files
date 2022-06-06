using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace Files.Infrastructure.Files;

public abstract class FileRepositoryBase : IDisposable
{
    protected FileRepositoryBase(IOptions<FilesOptions> filesOptions)
    {
        AmazonS3Client = new AmazonS3Client(
            filesOptions.Value.AccessKey,
            filesOptions.Value.SecretAccessKey,
            new AmazonS3Config { ServiceURL = filesOptions.Value.ServiceUrl });
    }

    protected AmazonS3Client AmazonS3Client { get; }
    
    protected async Task UploadAsync(
        string bucketName,
        string key,
        string contentType,
        byte[] content,
        CancellationToken cancellationToken = default)
    {
        await TryCreateFolderAsync(bucketName, GetFolderKey(), cancellationToken);
        
        await using var memoryStream = new MemoryStream(content);
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            ContentType = contentType,
            InputStream = memoryStream
        };

        await AmazonS3Client.PutObjectAsync(request, cancellationToken);

        string GetFolderKey()
        {
            var keyParts = key.Split("/");
            return string.Join("/", keyParts.Take(keyParts.Length - 1)) + "/";
        }
    }

    private async Task TryCreateFolderAsync(
        string bucketName,
        string key,
        CancellationToken cancellationToken = default)
    {
        var findFolderRequest = new ListObjectsV2Request
        {
            BucketName = bucketName,
            Prefix = key,
            MaxKeys = 1
        };
        
        var findFolderResponse = await AmazonS3Client.ListObjectsV2Async(findFolderRequest, cancellationToken);
        if (findFolderResponse.S3Objects.Any())
            return;
        
        var createFolderRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key
        };
        
        await AmazonS3Client.PutObjectAsync(createFolderRequest, cancellationToken);
    }

    protected async Task<(string ContentType, byte[] Content)?> DownloadAsync(
        string bucketName,
        string key,
        CancellationToken cancellationToken)
    {
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = key
        };

        try
        {
            using var response = await AmazonS3Client.GetObjectAsync(request, cancellationToken);
            await using var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);
            var content = memoryStream.ToArray();
        
            return (response.Headers.ContentType, content);
        }
        catch (AmazonS3Exception ex) when(ex.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }
    }
    
    protected async Task DeleteAsync(
        string bucketName,
        string key,
        CancellationToken cancellationToken)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = key
        };

        await AmazonS3Client.DeleteObjectAsync(request, cancellationToken);
    }

    public void Dispose()
    {
        AmazonS3Client.Dispose();
        GC.SuppressFinalize(this);
    }
}
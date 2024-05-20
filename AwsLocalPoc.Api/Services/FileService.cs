using Amazon.S3;
using Amazon.S3.Model;

namespace AwsLocalPoc.Api.Services;

public class FileService : IFileService
{
    private readonly IAmazonS3 _amazonS3;

    public FileService(IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
    }

    public async Task<GetObjectResponse?> GetFile(string bucketName, string fileName)
    {
        var bucketExists = await _amazonS3.DoesS3BucketExistAsync(bucketName);

        if (!bucketExists)
        {
            return null;
        }

        try
        {
            var getObjectResponse = await _amazonS3.GetObjectAsync(bucketName,
                fileName);

            return getObjectResponse;
        }
        catch (AmazonS3Exception ex) when (ex.ErrorCode.Equals("NotFound", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
    }

    public async Task<bool> UploadFile(IFormFile file, string bucketName)
    {
        var bucketExists = await _amazonS3.DoesS3BucketExistAsync(bucketName);

        if (!bucketExists)
        {
            return false;
        }

        using var fileStream = file.OpenReadStream();

        var extension = Path.GetExtension(file.FileName);

        var fileName = $"{Guid.NewGuid().ToString()}{extension}";

        var putObjectRequest = new PutObjectRequest()
        {
            BucketName = bucketName,
            Key = fileName,
            InputStream = fileStream
        };

        putObjectRequest.Metadata.Add("Content-Type", file.ContentType);

        var putResult = await _amazonS3.PutObjectAsync(putObjectRequest);

        return true;
    }
}
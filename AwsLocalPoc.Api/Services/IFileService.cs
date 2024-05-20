using Amazon.S3.Model;

namespace AwsLocalPoc.Api.Services;

public interface IFileService
{
    Task<GetObjectResponse?> GetFile(string bucketName, string fileName);
    Task<bool> UploadFile(IFormFile file, string bucketName);
}
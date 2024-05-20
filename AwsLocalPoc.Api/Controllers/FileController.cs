using Amazon.S3;
using Amazon.S3.Model;
using AwsLocalPoc.Api.Services;
using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.Mvc;

namespace AwsLocalPoc.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file, [FromForm] string bucketName)
    {
        var result = await _fileService.UploadFile(file, bucketName);

        if (result)
            return Ok($"File {file.Name} uploaded");

        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetFile(string bucketName, string key)
    {
        var getObjectResponse = await _fileService.GetFile(bucketName, key);

        if (getObjectResponse is null)
            return NotFound();

        return File(getObjectResponse.ResponseStream,
            getObjectResponse.Headers.ContentType);
    }
}
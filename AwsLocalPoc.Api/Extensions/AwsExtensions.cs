using Amazon;
using Amazon.S3;

namespace AwsLocalPoc.Api.Extensions;

public static class AwsExtensions
{
    public static void AddAwsS3Service(this WebApplicationBuilder builder)
    {
        if (builder.Configuration.GetSection("AWS") is null)
        {
            builder.Services.AddAWSService<IAmazonS3>();
        }
        else
        {
            builder.Services.AddSingleton<IAmazonS3>(sc =>
            {
                var accessKey = builder.Configuration["AWS:AccessKey"];
                var secretKey = builder.Configuration["AWS:SecretKey"];
                var awsS3Config = new AmazonS3Config
                {
                    UseHttp = true,
                    RegionEndpoint = RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"]),
                    ForcePathStyle = bool.Parse(builder.Configuration["AWS:ForcePathStyle"]!)
                };

                if (sc.GetService<IHostEnvironment>().IsDevelopment())
                {
                    awsS3Config.ServiceURL = builder.Configuration["AWS:ServiceURL"];
                    awsS3Config.ForcePathStyle = true;
                }

                return new AmazonS3Client(accessKey, secretKey, awsS3Config);
            });
        }
    }
}
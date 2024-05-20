# Instalação usando docker
```
mkdir -p ~/localstack/data
```

~~~docker
version: "3.8"

services:
  localstack:
    container_name: "${LOCALSTACK_DOCKER_NAME:-localstack-main}"
    image: localstack/localstack
    ports:
      - "127.0.0.1:4566:4566"            # LocalStack Gateway
      - "127.0.0.1:4510-4559:4510-4559"  # external services port range
    environment:
      # LocalStack configuration: https://docs.localstack.cloud/references/configuration/
      - DEBUG=${DEBUG:-0}
    volumes:
      - "~/localstack/data:/var/lib/localstack"
      - "/var/run/docker.sock:/var/run/docker.sock"
~~~

## Configurar .net

Adicionar o package

```
dotnet add package AWSSDK.S3
```
```
dotnet add package AWSSDK.Extensions.NETCore.Setup
```

## Configurar startup

~~~chsarp
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
~~~

O accessKey e o secretKey podem ser qualquer valor, pelo fato de usarmos localstack.

A implementação é a mesma que usaremos futuramente para conectar no S3.

using AwsLocalPoc.Api.Extensions;
using AwsLocalPoc.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddAwsS3Service();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

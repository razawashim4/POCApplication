using Microsoft.EntityFrameworkCore;
using POC.API.Middleware;
using POC.Application.IServices;
using POC.Application.Mapping;
using POC.Application.Services;
using POC.Domain.IRepositories;
using POC.Infrastructure;
using POC.Infrastructure.BackgroungTask;
using POC.Infrastructure.Implementation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("IsRequestLog"))
        .WriteTo.File(
            "Logs/Requests/request-.log",
            rollingInterval: RollingInterval.Day,
            fileSizeLimitBytes: 9 * 1024 ,
            rollOnFileSizeLimit: true,
            buffered: false
        ))
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("IsExceptionLog"))
        .WriteTo.File(
            "Logs/Exceptions/exception-.log",
            rollingInterval: RollingInterval.Day,
            fileSizeLimitBytes: 9 * 1024 ,
            rollOnFileSizeLimit: true,
            buffered: false
        ))

    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();


builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("con")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductService, ProductServices>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IAzureBlobService, AzureBlobService>();

builder.Services.AddHostedService<FileUploadBackgroundService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAngular");
app.UseMiddleware<RequestLogMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();
app.Run();

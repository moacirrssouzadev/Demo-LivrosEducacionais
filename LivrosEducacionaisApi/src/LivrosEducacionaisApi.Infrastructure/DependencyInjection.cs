using LivrosEducacionaisApi.Application.Interfaces;
using LivrosEducacionaisApi.Domain.Repositories;
using LivrosEducacionaisApi.Infrastructure.Data;
using LivrosEducacionaisApi.Infrastructure.Repositories;
using LivrosEducacionaisApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using Azure.Core.Pipeline;

namespace LivrosEducacionaisApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(30);
                }));

        // Repositories
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookVersionRepository, BookVersionRepository>();

        // Redis Cache - Optimizado para velocidade
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{configuration.GetConnectionString("Redis")},connectTimeout=5000,responseTimeout=5000,syncTimeout=5000,allowAdmin=true,abortConnect=false";
        });
        services.AddScoped<ICacheService, RedisCacheService>();

        // Blob Storage
        var blobConnectionString = configuration.GetConnectionString("BlobStorage") 
            ?? configuration["AzureStorage:ConnectionString"];
        var blobContainerName = configuration["BlobStorage:ContainerName"] ?? "book-covers";

        services.AddSingleton(sp => new BlobServiceClient(blobConnectionString, new BlobClientOptions
        {
            Retry =
            {
                MaxRetries = 3,
                Mode = Azure.Core.RetryMode.Exponential,
                Delay = TimeSpan.FromSeconds(1),
                MaxDelay = TimeSpan.FromSeconds(5)
            },
            Transport = new HttpClientTransport(new HttpClient { Timeout = TimeSpan.FromSeconds(30) })
        }));
        services.AddScoped<IBlobStorageService>(sp => 
            new BlobStorageService(sp.GetRequiredService<BlobServiceClient>(), blobContainerName));

        return services;
    }
}

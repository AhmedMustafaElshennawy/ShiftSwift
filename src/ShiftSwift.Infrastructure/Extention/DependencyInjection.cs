using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Application.services.Email;
using ShiftSwift.Application.settings;
using ShiftSwift.Infrastructure.services.Authentication;
using ShiftSwift.Infrastructure.services.Email;
using System.Text;
using Azure.Storage.Blobs;
using ShiftSwift.Application.services.MediaService;
using ShiftSwift.Infrastructure.services.MediaService;

namespace ShiftSwift.Infrastructure.Extention;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddJwt(configuration)
            .RegisterEmailServices(configuration)
            .RegisterAzureServices(configuration)
            .RegisterAzureServices(configuration)
            .AddApplicationServices();

        return services;
    }

    private static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        JwtSettings jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidIssuer = configuration[jwtSettings.Issuer],
                ValidateAudience = false,
                ValidAudience = configuration[jwtSettings.Audience],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddScoped<ITokenGenerator, TokenGenerator>();
        return services;
    }

    private static IServiceCollection RegisterEmailServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IEmailService, EmailService>();
        var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        return services;
    }

    private static IServiceCollection RegisterAzureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var azureSettings = configuration.GetSection(AzureBlobStorageSettings.SectionName)
            .Get<AzureBlobStorageSettings>();

        AzureBlobStorageSettings azureBlobStorageSettings = new();
        if (azureSettings is null)
        {
            throw new InvalidOperationException("Azure Blob Storage settings not configured");
        }

        configuration.Bind(AzureBlobStorageSettings.SectionName,azureBlobStorageSettings);

        // Register BlobServiceClient as singleton (recommended by Azure SDK)
        services.AddSingleton(x => new BlobServiceClient(azureSettings.ConnectionString));

        // Register a factory for BlobContainerClient
        services.AddScoped(x =>
        {
            var blobServiceClient = x.GetRequiredService<BlobServiceClient>();
            var containerClient = blobServiceClient.GetBlobContainerClient(azureSettings.ContainerName);
            return containerClient;
        });

        // Register your service
        services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();
        services.AddScoped<IMediaService, MediaService>();

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        return services;
    }
}
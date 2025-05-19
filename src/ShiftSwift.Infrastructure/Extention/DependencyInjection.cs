using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShiftSwift.API.Services;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Application.services.Email;
using ShiftSwift.Application.settings;
using ShiftSwift.Infrastructure.services.Authentication;
using ShiftSwift.Infrastructure.services.Email;
using System.Text;

namespace ShiftSwift.Infrastructure.Extention;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        JwtSettings jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(options =>
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


        services.AddTransient<IEmailService, EmailService>();
        var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        return services;
    }
}
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShiftSwift.API.Services;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Application.services.caching;
using ShiftSwift.Application.services.Email;
using ShiftSwift.Application.settings;
using ShiftSwift.Infrastructure.services.Authentication;
using ShiftSwift.Infrastructure.services.caching;
using ShiftSwift.Infrastructure.services.Email;
using System.Text;

namespace ShiftSwift.Infrastructure.Extention
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
   
            services.AddTransient<IEmailService, EmailService>();
            var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<ITokenGenerator, TokenGenerator>();

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            JwtSettings _jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, _jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = configuration[_jwtSettings.Issuer],
                    ValidAudience = configuration[_jwtSettings.Audience],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
            return services;
        }
    }
}

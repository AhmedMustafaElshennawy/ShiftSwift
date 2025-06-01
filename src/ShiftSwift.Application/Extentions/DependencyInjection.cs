using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftSwift.Application.Behaviors;
using System.Reflection;
using Mapster;
using MapsterMapper;

namespace ShiftSwift.Application.Extentions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatRConfiguration()
            .AddMapping();

        return services;
    }

    private static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return services;
    }

    private static IServiceCollection AddMapping(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);
        services.AddSingleton<IMapper, ServiceMapper>();

        return services;
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Domain.identity;
using ShiftSwift.Presistence.Common.Repository;
using ShiftSwift.Presistence.Common.UnitOfWork;
using ShiftSwift.Presistence.Context;


namespace ShiftSwift.Presistence.Extentions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlDbConfiguration(configuration)
            .AddDataAcessTypesToDiContainer();

        return services;
    }

    private static IServiceCollection AddSqlDbConfiguration(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<ShiftSwiftDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(ShiftSwiftDbContext).Assembly.FullName)
            ));

        services.AddIdentity<Account, IdentityRole>()
            .AddEntityFrameworkStores<ShiftSwiftDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
    private static IServiceCollection AddDataAcessTypesToDiContainer(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

        return services;
    }
}
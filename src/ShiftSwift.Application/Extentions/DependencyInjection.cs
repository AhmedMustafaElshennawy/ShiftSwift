using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftSwift.Application.Behaviors;
using ShiftSwift.Application.Resolver;

namespace GymManagement.Application.Extention
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicaion(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
                options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
            services.AddControllersWithViews();

            services.AddSingleton<AccountPictureResolver>();


            return services;
        }
    }
}
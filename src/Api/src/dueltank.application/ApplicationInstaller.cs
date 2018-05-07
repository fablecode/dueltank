using System.Reflection;
using dueltank.application.Configuration;
using dueltank.Domain.Service;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace dueltank.application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddCqrs();

            services.AddTransient<IEmailConfiguration, EmailConfiguration>();

            return services;
        }

        public static IServiceCollection AddCqrs(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationInstaller).GetTypeInfo().Assembly);

            return services;
        }
    }
}
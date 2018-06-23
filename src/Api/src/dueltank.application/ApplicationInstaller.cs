using System.Reflection;
using dueltank.application.Commands.UploadYgoProDeck;
using dueltank.application.Configuration;
using dueltank.Domain.Service;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace dueltank.application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailConfiguration, EmailConfiguration>();

            services.AddCqrs();
            services.AddValidation();

            return services;
        }

        public static IServiceCollection AddCqrs(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationInstaller).GetTypeInfo().Assembly);

            return services;
        }

        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<UploadYgoProDeckCommand>, UploadYgoProDeckCommandValidator>();

            return services;
        }

    }
}
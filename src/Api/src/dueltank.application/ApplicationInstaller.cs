using System.Reflection;
using dueltank.application.Commands.UploadYgoProDeck;
using dueltank.application.Configuration;
using dueltank.application.Validations.Deck;
using dueltank.core.Models.YgoPro;
using dueltank.core.Services;
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
            services.AddDomainServices();

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
            services.AddTransient<IValidator<YgoProDeck>, YgoProDeckValidator>();

            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IDeckService, DeckService>();
            services.AddTransient<ICardService, CardService>();
            services.AddTransient<IDeckTypeService, DeckTypeService>();
            services.AddTransient<IFormatService, FormatService>();
            services.AddTransient<ICategoryService, CategoryService>();

            return services;
        }
    }
}
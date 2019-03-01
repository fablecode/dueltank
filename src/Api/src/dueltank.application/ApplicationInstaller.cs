using AutoMapper;
using dueltank.application.Commands.UploadYgoProDeck;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Decks;
using dueltank.application.Validations.Decks.YgoProDeck;
using dueltank.core.Models.YgoPro;
using dueltank.core.Services;
using dueltank.Domain.Service;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace dueltank.application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddCqrs();
            services.AddValidation();
            services.AddDomainServices();

            services.AddAutoMapper();

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
            services.AddTransient<IValidator<DeckInputModel>, DeckValidator>();
            services.AddTransient<IValidator<DeckThumbnailInputModel>, DeckThumbnailValidator>();

            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IDeckService, DeckService>();
            services.AddTransient<ICardService, CardService>();
            services.AddTransient<IDeckTypeService, DeckTypeService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ISubCategoryService, SubCategoryService>();
            services.AddTransient<IAttributeService, AttributeService>();
            services.AddTransient<ITypeService, TypeService>();
            services.AddTransient<IFormatService, FormatService>();
            services.AddTransient<ILimitService, LimitService>();
            services.AddTransient<ITipService, TipService>();
            services.AddTransient<IRulingService, RulingService>();
            services.AddTransient<IBanlistService, BanlistService>();
            services.AddTransient<IArchetypeService, ArchetypeService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IFileService, FileService>();

            return services;
        }
    }
}
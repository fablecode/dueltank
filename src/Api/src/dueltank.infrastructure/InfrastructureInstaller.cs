using dueltank.core.Services;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using dueltank.Domain.SystemIO;
using dueltank.infrastructure.Database;
using dueltank.infrastructure.Repository;
using dueltank.infrastructure.SystemIO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace dueltank.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastuctureServices(this IServiceCollection services, string connectionString)
        {
            services.AddInfrastructureServices();
            services.AddDueltankDatabase(connectionString);
            services.AddRepositories();

            return services;
        }

        public static IServiceCollection AddDueltankDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<DueltankDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IDeckRepository, DeckRepository>();
            services.AddTransient<ICardRepository, CardRepository>();
            services.AddTransient<IDeckTypeRepository, DeckTypeRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IAttributeRepository, AttributeRepository>();
            services.AddTransient<ITypeRepository, TypeRepository>();
            services.AddTransient<IFormatRepository, FormatRepository>();
            services.AddTransient<ILimitRepository, LimitRepository>();
            services.AddTransient<ISubCategoryRepository, SubCategoryRepository>();
            services.AddTransient<ITipRepository, TipRepository>();
            services.AddTransient<IRulingRepository, RulingRepository>();
            services.AddTransient<IBanlistRepository, BanlistRepository>();
            services.AddTransient<IArchetypeRepository, ArchetypeRepository>();
            services.AddTransient<IDeckCardRepository, DeckCardRepository>();
            services.AddTransient<IUserRepository, UserRepository>();


            services.AddTransient<IDeckFileSystem, DeckFileSystem>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
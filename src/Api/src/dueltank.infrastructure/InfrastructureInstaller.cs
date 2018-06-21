using dueltank.Domain.Service;
using dueltank.infrastructure.Database;
using dueltank.infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace dueltank.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastuctureServices(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IEmailService, EmailService>();
            services.AddDbContextPool<DueltankDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
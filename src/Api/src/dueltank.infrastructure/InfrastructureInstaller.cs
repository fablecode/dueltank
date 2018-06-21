using dueltank.Domain.Service;
using dueltank.infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;

namespace dueltank.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastuctureServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailService>();
            
            return services;
        }
    }
}
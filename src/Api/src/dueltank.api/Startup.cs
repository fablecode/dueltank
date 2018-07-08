using System;
using System.IO;
using System.Reflection;
using dueltank.api.Auth.Swagger;
using dueltank.api.ServiceExtensions;
using dueltank.application;
using dueltank.application.Configuration;
using dueltank.Domain.Service;
using dueltank.infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace dueltank.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
                Configuration = builder.Build();
            }
            else
            {
                Configuration = configuration;
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerDocumentation();
            services.AddIdentityConfiguration(Configuration);

            services.Configure<ApplicationSettings>(Configuration.GetSection("AppSettings"));

            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddApplicationServices();
            services.AddInfrastuctureServices(Configuration.GetConnectionString("dueltank"));

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());

            services.AddMvc(setupAction =>
            {
                // 406 Not Acceptable response, if accept header not supported.
                setupAction.ReturnHttpNotAcceptable = true;

                // Xml Formatters support
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

                // Single authorization policy used globally
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                setupAction.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Dueltank API", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
                // To add an extra token input field in the Swagger UI
                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();

                var fileName = GetType().GetTypeInfo().Module.Name.Replace(".dll", ".xml").Replace(".exe", ".xml");
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, fileName));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseSwaggerDocumentation();
            app.AddIdentityConfiguration();
            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}

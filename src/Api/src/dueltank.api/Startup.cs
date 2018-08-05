﻿using dueltank.api.Auth.Swagger;
using dueltank.api.ServiceExtensions;
using dueltank.application;
using dueltank.application.Configuration;
using dueltank.Domain.Configuration;
using dueltank.infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Net;
using System.Reflection;

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

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
                    // Apply CORS policy for any type of origin  
                    .AllowAnyMethod()
                    // Apply CORS policy for any type of http methods  
                    .AllowAnyHeader()
                    // Apply CORS policy for any headers  
                    .AllowCredentials());
                // Apply CORS policy for all users  
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

            services.AddMvc(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
            });

            // HTTP Strict Transport Security (HSTS) configuration
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 443;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // HTTP Strict Transport Security (HSTS) enabled
                // Citation: https://dzone.com/articles/enforce-ssl-and-use-hsts-in-net-core20-net-core-se
                // Citation: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.1&tabs=visual-studio
                app.UseHsts();
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseCookiePolicy();

                app.UseExceptionHandler(
                    options =>
                    {
                        options.Run(
                            async context =>
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                context.Response.ContentType = "text/html";
                                var ex = context.Features.Get<IExceptionHandlerFeature>();
                                if (ex != null)
                                {
                                    var log = logger.CreateLogger<Startup>();
                                    var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace}";
                                    log.LogError(err);

                                    await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                                }
                            });
                    }
                );
            }


            app.UseSwaggerDocumentation();
            app.UseCors("AllowAll");
            app.AddIdentityConfiguration();
            app.UseMvc();
        }
    }
}

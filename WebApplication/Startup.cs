using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGrainFactory>(sp =>
            {
                var builder = new ClientBuilder().LoadConfiguration();
                var client = builder.Build();
                client.Connect().Wait();
                return client;
            });

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<FileOperation>();
                c.SchemaFilter<PropertySchema>();

                // Added for example, there's no auth in this solution
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Name = "Authorization",
                    In = "header"
                });

                // Added for example, there's no auth in this solution
                c.AddSecurityDefinition("Identity", new OAuth2Scheme
                {
                    AuthorizationUrl = "http://localhost:5000",
                    Flow = "Implicit",
                    TokenUrl = "http://localhost:5000/connect/token"
                });

                c.SwaggerDoc("api-v1", new Info());
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole().AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/api-v1/swagger.json", "Example API v1");
                });
            }

            app.ApplicationServices.GetService<IGrainFactory>();
            app.UseMvcWithDefaultRoute();
        }
    }
}
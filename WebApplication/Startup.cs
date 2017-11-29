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
using Orleans.Runtime;

namespace WebApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGrainFactory>(StartClientWithRetries().Result);
            services.AddMvc();
            //services.AddSwaggerGen(c =>
            //{
            //    c.OperationFilter<FileOperation>();
            //    c.SchemaFilter<PropertySchema>();

            //    // Added for example, there's no auth in this solution
            //    c.AddSecurityDefinition("Bearer", new ApiKeyScheme
            //    {
            //        Name = "Authorization",
            //        In = "header"
            //    });

            //    // Added for example, there's no auth in this solution
            //    c.AddSecurityDefinition("Identity", new OAuth2Scheme
            //    {
            //        AuthorizationUrl = "http://localhost:5000",
            //        Flow = "Implicit",
            //        TokenUrl = "http://localhost:5000/connect/token"
            //    });

            //    c.SwaggerDoc("api-v1", new Info());
            //});

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole().AddDebug();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c =>
            //    {
            //        c.SwaggerEndpoint("/swagger/api-v1/swagger.json", "Example API v1");
            //    });
            //}

            //app.ApplicationServices.GetService<IGrainFactory>();
            app.UseMvcWithDefaultRoute();
        }

        private static async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            await Task.Delay(3000);

            int attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    var builder = new ClientBuilder().LoadConfiguration();
                    client = builder.Build();
                    await client.Connect();
                    Console.WriteLine("===========>>> Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(4));
                }
            }

            return client;
        }
    }
}
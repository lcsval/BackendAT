using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Infra.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Api
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            Settings.OpenWeatherKey = $"{Configuration["OpenWeatherKey"]}";
            Settings.SpotifyClientId = $"{Configuration["SpotifyClientId"]}";
            Settings.SpotifyClientSecret = $"{Configuration["SpotifyClientSecret"]}";

            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            services
                .AddCors(options =>
                {
                    options.AddPolicy("AllowAll", corsBuilder.Build());
                })
                .AddMvc();

            services.AddSingleton<IConfiguration>(Configuration);

            // services.AddTransient<IUnitOfWork, UnitOfWork>();
            // services.AddTransient<IShelterReadRepository, ShelterReadRepository>();
            // services.AddTransient<IShelterWriteRepository, ShelterWriteRepository>();
            // services.AddTransient<IShelterCommandHandler, ShelterCommandHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");
            app.UseMvc();
        }
    }
}

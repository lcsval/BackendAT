using System.IO;
using Backend.Domain.Interfaces.Handlers;
using Backend.Domain.Interfaces.Infra;
using Backend.Domain.Interfaces.Repositories.Read;
using Backend.Domain.Interfaces.Repositories.Write;
using Backend.Handler.Handler;
using Backend.Infra.Repositories;
using Backend.Infra.Repositories.Read;
using Backend.Infra.Repositories.Write;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
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
            Settings.ConnectionString = $"{Configuration["ConnectionString"]}";

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

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IWeatherReadRepository, WeatherReadRepository>();
            services.AddTransient<IWeatherWriteRepository, WeatherWriteRepository>();
            services.AddTransient<IWeatherCommandHandler, WeatherCommandHandler>();
            services.AddTransient<ISongsCommandHandler, SongsCommandHandler>();
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

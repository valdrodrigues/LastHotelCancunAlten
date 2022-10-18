using System;
using System.IO;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using LastHotelCancunAlten.Application;
using LastHotelCancunAlten.Domain.Dto;
using LastHotelCancunAlten.Domain.IApplication;
using LastHotelCancunAlten.Domain.IRepository;
using LastHotelCancunAlten.Infra.Configuration;
using LastHotelCancunAlten.Infra.Repository;

namespace LastHotelCancunAlten
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // appsettings Config
            string basePath = Directory.GetCurrentDirectory();
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new HostBuilder()
                .ConfigureAppConfiguration(cb =>
                {
                    cb.SetBasePath(basePath)
                      .AddJsonFile($"appsettings.{environmentName}.json", false)
                      .AddEnvironmentVariables();
                });

            services.AddControllers();

            // Fluent Validation
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<ReservationDto>, ReservationDtoValidator>();

            // SwaggerGen Config
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Alten Test",
                    Description = "Booking API for the very last hotel in Cancun.",
                    Contact = new OpenApiContact
                    {
                        Name = "Valdeci Rodrigues Junior",
                        Url = new Uri("https://www.linkedin.com/in/valdeci-rodrigues-junior")
                    }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddSingleton(Configuration);

            // Application Dependency Injection 
            services.AddSingleton<IReservationApplication, ReservationApplication>();

            // MongoDB Config
            services.Configure<ReservationConfiguration>(Configuration.GetSection("CancunConnectionSettings"));
            services.AddSingleton<IReservationRepository, ReservationRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }
    }
}

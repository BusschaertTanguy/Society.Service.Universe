using System;
using Application.Configurations;
using Application.Queries;
using Application.Transactions;
using Consul;
using Domain.Repositories;
using Infrastructure.Dapper.Queries;
using Infrastructure.EntityFramework.Contexts;
using Infrastructure.EntityFramework.Repositories;
using Infrastructure.EntityFramework.Transactions;
using Infrastructure.MassTransit.Filters;
using Infrastructure.Queries;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("UniverseDb");
            services.AddDbContext<UniverseDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped(_ => new SqlDbConnectionProvider(connectionString));
        }

        public static void ConfigureUniverseServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationLayerConfiguration).Assembly);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUniverseRepository, UniverseRepository>();
            services.AddTransient<IUniverseQueries, UniverseQueries>();
        }

        public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            var username = configuration["MassTransit:Username"];
            var password = configuration["MassTransit:Password"];
            var host = configuration["MassTransit:Host"];

            services.AddMassTransit(cfg =>
            {
                cfg.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(host, "/", h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    configurator.UseConsumeFilter(typeof(ConsumerLogFilter<>), context);
                    configurator.UseSendFilter(typeof(SendLogFilter<>), context);
                    configurator.UsePublishFilter(typeof(PublishLogFilter<>), context);
                });
            });

            services.AddMassTransitHostedService();
        }
        
        public static void ConfigureConsul(this IServiceCollection services, IConfiguration configuration)
        {
            var consulClient = new ConsulClient(clientConfiguration => { clientConfiguration.Address = new Uri(configuration["Consul:Discovery"]); });
            services.AddSingleton<IConsulClient, ConsulClient>(_ => consulClient);
        }
    }
}
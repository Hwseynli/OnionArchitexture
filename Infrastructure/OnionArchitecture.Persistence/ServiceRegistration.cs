using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnionArchitecture.Application.Interfaces;
using OnionArchitecture.Persistence.Data;
using OnionArchitecture.Persistence.Repositories;

namespace OnionArchitecture.Persistence;
public static class ServiceRegistration
{
    public static void AddPersistenceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TestDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__efmigrationshistory", "testing");
                npgsqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(3), new List<string>());
            });
        });

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IRepository<>))
            .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(Repository<>))
            .AddClasses(classes => classes.AssignableTo(typeof(Repository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}

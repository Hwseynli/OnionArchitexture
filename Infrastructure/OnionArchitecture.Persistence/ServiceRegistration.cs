using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Application.Interfaces.IManagers;
using OnionArchitecture.Persistence.Concrete;
using OnionArchitecture.Persistence.Context;
using OnionArchitecture.Persistence.Repositories;
using OnionArchitecture.Application.Features.Queries;

namespace OnionArchitecture.Persistence;
public static class ServiceRegistration
{
    public static void AddPersistenceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TestDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), options =>
            {
                options.MigrationsHistoryTable("__efmigrationshistory", "testing");
                options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(3), new List<string>());
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

        services.AddScoped<IUserQueries, UserQueries>();
        services.AddScoped<IClaimManager, ClaimManager>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IEmailManager, EmailManager>();
        services.AddScoped<IDocumentManager, DocumentManager>();

    }
}

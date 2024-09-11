using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnionArchitecture.Application.Interfaces;
using OnionArchitecture.Persistence.Concrete;
using OnionArchitecture.Persistence.Context;
using OnionArchitecture.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArchitecture.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceRegistration(this IServiceCollection services)
        {

            services.AddDbContext<TestDbContext>(options =>
            {
                options.UseNpgsql("Host=135.181.251.254; Database=test; Username=elshanz; Password=LZ)3&nyzWEIx+bE8;SearchPath=testing", options =>
                {
                   options.MigrationsHistoryTable("__efmigrationshistory", "testing");
                    options.EnableRetryOnFailure(10,TimeSpan.FromSeconds(3),new List<string>());
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

            services.AddScoped<IClaimManager, ClaimManager>();
            services.AddScoped<IUserManager, UserManager>();
                
        }
    }
}

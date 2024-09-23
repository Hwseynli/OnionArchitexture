using OnionArchitecture.Domain.Entities.Roles;
using OnionArchitecture.Persistence.Context;
using Polly;
using Polly.Retry;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace OnionArchitecture.Infrastructure.SeedDatas
{
    public class TestSeedDbContext
    {
        public async Task SeedAsync(TestDbContext context, ILogger<TestSeedDbContext> logger)
        {
            var policy = CreatePolicy(logger, nameof(TestSeedDbContext));
            using (context)
            {
                await policy.ExecuteAsync(async () =>
                {
                    await RoleSeedAsync(context);
                    if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
                });
            }
        }

        private async Task RoleSeedAsync(TestDbContext context)
        {
            var roleJson = File.ReadAllText("./Infrastructure/SeedDatas/role.json");
            var roles = JsonConvert.DeserializeObject<List<Role>>(roleJson);
            foreach (var r in roles)
            {
                var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == r.Id);
                if (role is not null)
                {
                    role.SetDetails(r.Name);
                    context.Roles.Update(role);
                }
                else
                {
                    context.Roles.Add(r);
                }
            }
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<TestSeedDbContext> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().WaitAndRetryAsync(

                retries,
                retry => TimeSpan.FromSeconds(5),
                (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogTrace($"{prefix} Exception {exception.GetType().Name} with message {exception.Message} detected on attemt {retry} of {retries}");
                });
        }
    }
}





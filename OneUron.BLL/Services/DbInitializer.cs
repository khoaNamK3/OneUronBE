using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OneUron.DAL.Data;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public interface IDbInitializer
    {
        void Initialize();
        Task InitializeAsync();
    }

    public class DbInitializer : IDbInitializer, IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(IServiceScopeFactory scopeFactory, ILogger<DbInitializer> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public void Initialize()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                context.Database.Migrate();
                SeedRoles(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task InitializeAsync()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                await context.Database.MigrateAsync();
                await SeedRolesAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private void SeedRoles(AppDbContext context)
        {
            if (!context.Roles.Any())
            {
                _logger.LogInformation("Seeding roles");

                var roles = new List<Role>
                {
                    new Role { Id = Guid.NewGuid(), RoleName = "Admin", IsDeleted = false },
                    new Role { Id = Guid.NewGuid(), RoleName = "User", IsDeleted = false },
                    new Role { Id = Guid.NewGuid(), RoleName = "Instructor", IsDeleted = false }
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();

                _logger.LogInformation("Roles seeded successfully");
            }
            else
            {
                _logger.LogInformation("Roles already exist in the database");
            }
        }

        private async Task SeedRolesAsync(AppDbContext context)
        {
            if (!await context.Roles.AnyAsync())
            {
                _logger.LogInformation("Seeding roles asynchronously");
                
                var roles = new List<Role>
                {
                    new Role { Id = Guid.NewGuid(), RoleName = "Admin", IsDeleted = false },
                    new Role { Id = Guid.NewGuid(), RoleName = "User", IsDeleted = false },
                    new Role { Id = Guid.NewGuid(), RoleName = "Teacher", IsDeleted = false },
                    new Role { Id = Guid.NewGuid(), RoleName = "Student", IsDeleted = false }
                };

                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
                
                _logger.LogInformation("Roles seeded successfully");
            }
            else
            {
                _logger.LogInformation("Roles already exist in the database");
            }
        }

        // Required IHostedService implementation methods
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Initialize the database when the application starts
            return InitializeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // No cleanup needed
            return Task.CompletedTask;
        }
    }
}

namespace BetINK.Web.Infrastructure.Extensions
{
    using BetINK.Common.Constants;
    using BetINK.DataAccess.Models;
    using BetINK.Web.Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<BetINKDbContext>().Database.Migrate();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Task
                    .Run(async () =>
                    {
                        var roles = new[]
                        {
                            WebConstants.AdministratorRole,
                            WebConstants.ModeratorRole,
                        };

                        foreach (var role in roles)
                        {
                            var roleExists = await roleManager.RoleExistsAsync(role);

                            if (!roleExists)
                            {
                                await roleManager.CreateAsync(new IdentityRole
                                {
                                    Name = role
                                });
                            }
                        }

                        var adminUser = await userManager.FindByEmailAsync(WebConstants.DefaultEmail);

                        if (adminUser == null)
                        {
                            adminUser = new User
                            {
                                Email = WebConstants.DefaultEmail,
                                UserName = WebConstants.DefaultUser,
                                FirstName = Guid.NewGuid().ToString().Substring(0, 10),
                                LastName = Guid.NewGuid().ToString().Substring(0, 10)
                            };

                            await userManager.CreateAsync(adminUser, WebConstants.DefaultPassword);
                            await userManager.AddToRoleAsync(adminUser, WebConstants.AdministratorRole);
                        }
                    }).Wait();
            }
            return app;
        }
    }
}
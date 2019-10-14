using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DotNet_Core_Token_Authentication.DataAccesLayer
{
    public class SeedDataBase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = "lefajele@gmail.com",
                    NormalizedEmail = "lefajele@gmail.com",
                    NormalizedUserName = "lefajele",
                    PhoneNumber = "694522563",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "lefajele"
                };
                userManager.CreateAsync(user, "Password@123*");
            }
        }
    }
}

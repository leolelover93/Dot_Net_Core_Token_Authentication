﻿using Microsoft.AspNetCore.Identity;
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
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = "Admin"
                };

                roleManager.CreateAsync(identityRole);

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

                userManager.AddToRoleAsync(user, "Admin");
                
            }
        }
    }
}

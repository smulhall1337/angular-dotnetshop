using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                AppUser user = new AppUser
                {
                    DisplayName = "Bob",
                    Email = "Bob@test.com",
                    UserName = "bob@test.com",
                    Address = new Address
                    {
                        FirstName = "Bob",
                        LastName = "Bobbity",
                        Street = "123 fake st",
                        City = "New York",
                        State = "NY",
                        ZipCode = "90210"
                    }
                };
                await userManager.CreateAsync(user, "P@ssw0rd");
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Identity;

namespace Talabat.Repository.Identity
{
    public static class ApplicationIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {

            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Mariam",
                    Email = "MariamMahmoud2909@gmail.com",
                    UserName = "Mariam_Mahmoud",
                    PhoneNumber = "01022354027"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
					DisplayName = "Kareem",
					Email = "kareemtameregy@gmail.com",
					UserName = "kareem.tamer",
					PhoneNumber = "01025578893"
				}; 

				await userManager.CreateAsync(user, "Pa$$w0rd");
			}
		}
	}
}

using Microsoft.AspNetCore.Identity;
using TodoReact.Server.Models;

namespace TodoReact.Server.Services
{
	public static class IdentitySeeder
	{
		public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			var roles = new[]
			{
				"Admin",
				"User"
			};

			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
					await roleManager.CreateAsync(new ApplicationRole(role));
			}

			var adminEmail =  "adminEmail@gmail.com";
			var adminPassword = "Password123!";

			var userExists = await userManager.FindByEmailAsync(adminEmail);
			if (userExists == null)
			{
				var adminUser = new ApplicationUser
				{
					UserName = "Admin",
					Email = adminEmail,
					DisplayName = "Admin"
				};

				var result = await userManager.CreateAsync(adminUser, adminPassword);
				if (result.Succeeded)
					await userManager.AddToRoleAsync(adminUser, "Admin");
				else
					throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors));
			}
		}
	}
}

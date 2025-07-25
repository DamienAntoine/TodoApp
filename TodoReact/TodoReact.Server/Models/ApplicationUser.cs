using Microsoft.AspNetCore.Identity;

namespace TodoReact.Server.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string? DisplayName { get; set; }
	}
}

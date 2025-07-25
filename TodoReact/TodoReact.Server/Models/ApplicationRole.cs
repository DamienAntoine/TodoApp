using Microsoft.AspNetCore.Identity;

namespace TodoReact.Server.Models
{
	public class ApplicationRole : IdentityRole
	{
		public ApplicationRole() : base()
		{}

		public ApplicationRole(string roleName) : base(roleName)
		{}
	}
}

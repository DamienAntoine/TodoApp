using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace TodoReact.Server.Models
{
	public class TodoContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
	{
		public TodoContext(DbContextOptions<TodoContext> options) : base(options)
		{}

		public DbSet<TodoItem> ToDoItems { get; set; } = null!;
	}
}

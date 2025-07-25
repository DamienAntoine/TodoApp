using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TodoReact.Server.DTOs;
using TodoReact.Server.Enums;
using TodoReact.Server.Models;

namespace TodoReact.Server.Services
{
	public class AppUserManager
	{
		private readonly UserManager<ApplicationUser> _userManager;
		public AppUserManager(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<IEnumerable<UserDto>> GetFilteredUsersAsync(UserRole? role, string? username, string? displayname, SortUsersOption? sortby)
		{
			List<ApplicationUser> users;
			var query = _userManager.Users.AsQueryable();

			if (!string.IsNullOrWhiteSpace(username))
			{
				query = query.Where(u => u.UserName != null && u.UserName.Contains(username));
			}
			if (!string.IsNullOrWhiteSpace(displayname))
			{
				query = query.Where(u => u.DisplayName != null && u.DisplayName.Contains(displayname));
			}
			if (role.HasValue)
			{
				var usersInRole = await _userManager.GetUsersInRoleAsync(role.Value.ToString());
				var usersIdsInRole = usersInRole.Select(u => u.Id).ToList();
				query = query.Where(u => usersIdsInRole.Contains(u.Id));
			}

			query = sortby switch
			{
				SortUsersOption.Username => query.OrderBy(u => u.UserName),
				SortUsersOption.Displayname => query.OrderBy(u => u.DisplayName),
				_ => query
			};

			users = await query.ToListAsync();

			return users.Select(u => new UserDto
			{
				Id = u.Id,
				Username = u.UserName,
				DisplayName = u.DisplayName
			});
		}

		public async Task<List<UserDto>> GetUsersInRoleAsync(UserRole role)
		{
			var users = await _userManager.GetUsersInRoleAsync(role.ToString());
			return users.Select(u => new UserDto
			{
				Id = u.Id,
				Username = u.UserName,
				DisplayName = u.DisplayName
			}).ToList();
		}

		public async Task<UserDto?> GetUserByIdAsync(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
				return null;

			return new UserDto
			{
				Id = user.Id,
				Username = user.UserName,
				DisplayName = user.DisplayName
			};
		}

		public async Task<IdentityResult> CreateUserAsync(RegisterDto dto)
		{
			var user = new ApplicationUser
			{
				UserName = dto.Username,
				DisplayName = dto.DisplayName
			};

			var result = await _userManager.CreateAsync(user, dto.Password);
			if (!result.Succeeded)
				return result;

			result = await _userManager.AddToRoleAsync(user, UserRole.User.ToString());
			return result;
		}

		public async Task<IdentityResult> DeleteUserAsync(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
				return IdentityResult.Failed(new IdentityError { Description = "User not found." });

			var result = await _userManager.DeleteAsync(user);
			return result;
		}
	}
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoReact.Server.DTOs;
using TodoReact.Server.Enums;
using TodoReact.Server.Models;
using TodoReact.Server.Services;

namespace TodoReact.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : Controller
	{
		private readonly AppUserManager _appUserManager;
		private readonly SignInManager<ApplicationUser> _SignInManager;

		public AccountController(AppUserManager appUserManager, SignInManager<ApplicationUser> signInManager)
		{
			_appUserManager = appUserManager;
			_SignInManager = signInManager;
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
			[FromQuery] UserRole? role,
			[FromQuery] string? username,
			[FromQuery] string? displayname,
			[FromQuery] SortUsersOption? sortby)
		{
			var users = await _appUserManager.GetFilteredUsersAsync(role, username, displayname, sortby);
			if (!users.Any())
				return NotFound();

			return Ok(users);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("{id}")]
		public async Task<ActionResult<UserDto>> GetUserById(string id)
		{
			var user = await _appUserManager.GetUserByIdAsync(id);
			if (user == null)
				return NotFound("User not found.");

			return user;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			var result = await _appUserManager.CreateUserAsync(dto);

			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Created();
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			if (string.IsNullOrWhiteSpace(dto.Username))
				return BadRequest("Username is required.");
			var result = await _SignInManager.PasswordSignInAsync(dto.Username, dto.Password, true, false);
			if (!result.Succeeded)
				return Unauthorized();
			return Ok();
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var result = await _appUserManager.DeleteUserAsync(id);
			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Ok();
		}
	}
}

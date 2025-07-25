namespace TodoReact.Server.DTOs
{
	public class RegisterDto
	{
		public string? Username { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string? DisplayName { get; set; } = null!;
	}
}

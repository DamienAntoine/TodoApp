namespace TodoReact.Server.DTOs
{
	public class UserDto
	{
		public string Id { get; set; } = null!;
		public string? Username { get; set; } = null!;
		public string? DisplayName { get; set; }
	}
}

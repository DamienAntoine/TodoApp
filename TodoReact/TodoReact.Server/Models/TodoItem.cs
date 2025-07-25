namespace TodoReact.Server.Models
{
	public class TodoItem
	{
		public Guid Id { get; set; }
		public required string Name { get; set; }
		public string? Description { get; set; }
		public string? Tag { get; set; }
		public bool IsComplete { get; set; }
		public string? Secret { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeadLine { get; set; }
		public int Priority { get; set; }
		public string OwnerId { get; set; } = null!;
		public ApplicationUser Owner { get; set; } = null!;
	}
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoReact.Server.DTOs;
using TodoReact.Server.Enums;
using TodoReact.Server.Services;

namespace TodoReact.Server.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class TodoItemsController : ControllerBase
	{
		private readonly TaskManager _taskManager;

		public TodoItemsController(TaskManager taskManager)
		{
			_taskManager = taskManager;
		}

		// GET: api/TodoItems
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetToDoItems(
			[FromQuery] PriorityLevel? priority,
			[FromQuery] bool? isComplete,
			[FromQuery] string? tag,
			[FromQuery] string? keyword,
			[FromQuery] SortByOption? sortBy,
			[FromQuery] string? order
		)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized("User is not authenticated.");

			var items = await _taskManager.GetFilteredTodoItems(userId, priority, isComplete, tag, keyword, sortBy, order);

			if (items == null || !items.Any())
				return NotFound();

			return Ok(items);
		}

		// GET: api/TodoItems/5
		[HttpGet("{id}")]
		public async Task<ActionResult<TodoItemDTO>> GetTodoItem(Guid id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized("User is not authenticated.");

			var result = await _taskManager.GetTodoItem(id, userId);
			if (result == null)
				return NotFound();
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized("User is not authenticated.");

			todoDTO.OwnerId = userId;

			var (success, error, item) = await _taskManager.AddTodoItemAsync(todoDTO);
			if (!success)
				return BadRequest(error);

			return CreatedAtAction(nameof(GetTodoItem), new { id = item!.Id }, item);
		}

		// PUT: api/TodoItems/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutTodoItem(Guid id, TodoItemDTO todoDTO)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized("User is not authenticated.");

			var existing = await _taskManager.GetTodoItem(id, userId);
			if (existing == null)
				return NotFound();

			todoDTO.OwnerId = userId;

			var (success, error) = await _taskManager.UpdateTodoItemAsync(id, todoDTO);
			if (!success)
			{
				if (error == "Not found")
					return NotFound();
				return BadRequest(error);
			}

			return NoContent();
		}

		// DELETE: api/TodoItems/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTodoItem(Guid id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized("User is not authenticated.");

			var existing = await _taskManager.GetTodoItem(id, userId);
			if (existing == null)
				return NotFound();

			var success = await _taskManager.DeleteTodoItem(id);
			if (!success)
			{
				return NotFound();
			}
			return NoContent();
		}
	}
}

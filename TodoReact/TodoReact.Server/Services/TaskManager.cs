using Microsoft.EntityFrameworkCore;
using TodoReact.Server.DTOs;
using TodoReact.Server.Enums;
using TodoReact.Server.Models;


namespace TodoReact.Server.Services
{
	public class TaskManager
	{
		private readonly TodoContext _context;
		public TaskManager(TodoContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<TodoItemDTO>>GetFilteredTodoItems(string ownerId, PriorityLevel? priority, bool? isComplete, string? tag, string? keyword, SortByOption? sortBy, string? order)
		{
			var query = _context.ToDoItems.Where(x => x.OwnerId == ownerId);
			if (priority.HasValue)
			{
				query = query.Where(x => x.Priority == (int)priority.Value);
			}

			if (isComplete.HasValue)
			{
				IQueryable<TodoItem> todoItems = query.Where(x => x.IsComplete == isComplete.Value);
				query = todoItems;
			}

			if (!string.IsNullOrWhiteSpace(tag))
			{
				query = query.Where(x => x.Tag != null && x.Tag.Contains(tag));
			}

			if (!string.IsNullOrWhiteSpace(keyword))
			{
				query = query.Where(x =>
									(x.Description != null && x.Description.Contains(keyword)) ||
									(x.Name != null && x.Name.Contains(keyword)));
			}

			bool descending = !string.IsNullOrWhiteSpace(order) && order == "desc";
			query = sortBy switch
			{
				SortByOption.Name => descending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
				SortByOption.Description => descending ? query.OrderByDescending(x => x.Description) : query.OrderBy(x => x.Description),
				SortByOption.Priority => descending ? query.OrderByDescending(x => x.Priority) : query.OrderBy(x => x.Priority),
				SortByOption.Done => descending ? query.OrderByDescending(x => x.IsComplete) : query.OrderBy(x => x.IsComplete),
				SortByOption.Deadline => descending ? query.OrderByDescending(x => x.DeadLine) : query.OrderBy(x => x.DeadLine),
				SortByOption.Created => descending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
				SortByOption.Updated => descending ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
				_ => query
			};

			return await query.Select(x => ItemToDTO(x)).ToListAsync();
		}

		//return task by id
		public async Task<TodoItemDTO?> GetTodoItem(Guid id, string ownerId)
		{
			var todoItem = await _context.ToDoItems
				.Where(x => x.Id == id && x.OwnerId == ownerId)
				.FirstOrDefaultAsync();

			if (todoItem == null)
				return null;

			return ItemToDTO(todoItem);
		}

		//add task
		public async Task<(bool Success, string? Error, TodoItemDTO? Item)> AddTodoItemAsync(TodoItemDTO todoDTO)
		{
			if (await _context.ToDoItems.AnyAsync(x => x.Name == todoDTO.Name && x.Description == todoDTO.Description))
				return (false, "Cannot add duplicate tasks", null);

			if (string.IsNullOrWhiteSpace(todoDTO.Name))
				return (false, "Name cannot be empty", null);

			if (todoDTO.DeadLine.HasValue && todoDTO.DeadLine.Value < DateTime.UtcNow)
				return (false, "DeadLine cannot be in the past.", null);

			if (todoDTO.Priority > 3 || todoDTO.Priority < 1)
				return (false, "Priority has to be set between 1 and 3.", null);

			if (string.IsNullOrWhiteSpace(todoDTO.OwnerId))
				return (false, "OwnerId is required", null);

			var todoItem = new TodoItem
			{
				IsComplete = todoDTO.IsComplete,
				Name = todoDTO.Name,
				Description = todoDTO.Description,
				Tag = todoDTO.Tag,
				CreatedAt = DateTime.UtcNow,
				DeadLine = todoDTO.DeadLine,
				Priority = todoDTO.Priority,
				OwnerId = todoDTO.OwnerId
			};

			_context.ToDoItems.Add(todoItem);
			await _context.SaveChangesAsync();

			return (true, null, ItemToDTO(todoItem));
		}

		//update task
		public async Task<(bool Success, string? Error)> UpdateTodoItemAsync(Guid id, TodoItemDTO todoDTO)
		{
			if (id != todoDTO.Id)
				return (false, "Id mismatch");
			if (await _context.ToDoItems.AnyAsync(x => x.Id != id && x.Name == todoDTO.Name && x.Description == todoDTO.Description && x.OwnerId == todoDTO.OwnerId))
				return (false, "Cannot add duplicate tasks");

			if (string.IsNullOrWhiteSpace(todoDTO.Name))
				return (false, "Name cannot be empty");

			if (todoDTO.DeadLine.HasValue && todoDTO.DeadLine.Value < DateTime.UtcNow)
				return (false, "DeadLine cannot be in the past.");

			if (todoDTO.Priority > 3 || todoDTO.Priority < 1)
				return (false, "Priority has to be set between 1 and 3.");

			var todoItem = await _context.ToDoItems.FindAsync(id);
			if (todoItem == null)
				return (false, "Not found");

			todoItem.Name = todoDTO.Name;
			todoItem.Description = todoDTO.Description;
			todoItem.Tag = todoDTO.Tag;
			todoItem.IsComplete = todoDTO.IsComplete;
			todoItem.UpdatedAt = DateTime.UtcNow;
			todoItem.DeadLine = todoDTO.DeadLine;
			todoItem.Priority = todoDTO.Priority;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				return (false, "Not found");
			}

			return (true, null);
		}

		public async Task<bool> DeleteTodoItem(Guid id)
		{
			var todoItem = await _context.ToDoItems.FindAsync(id);
			if (todoItem == null)
			{
				return false;
			}

			_context.ToDoItems.Remove(todoItem);
			await _context.SaveChangesAsync();

			return true;
		}

		private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
			new TodoItemDTO
			{
				Id = todoItem.Id,
				Name = todoItem.Name,
				IsComplete = todoItem.IsComplete,
				Description = todoItem.Description,
				Tag = todoItem.Tag,
				CreatedAt = todoItem.CreatedAt,
				UpdatedAt = todoItem.UpdatedAt,
				DeadLine = todoItem.DeadLine,
				Priority = todoItem.Priority,
				OwnerId = todoItem.OwnerId
			};
	}
}

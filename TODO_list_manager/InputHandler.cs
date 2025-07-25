using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_list_manager
{
	internal class InputHandler
	{
		private readonly TaskManager manager = new();

		private void AddTaskFromInput()
		{
			Console.Write("Task description : ");
			string? description = Console.ReadLine();
			if (!string.IsNullOrWhiteSpace(description))
				manager.AddTask(description);
		}

		private void DeleteTaskFromInput()
		{
			Console.WriteLine("Task index to delete : ");
			manager.ShowTasks();
			string? toDeleteIndex = Console.ReadLine();
			if (int.TryParse(toDeleteIndex, out int deleteIndex))
			{
				if (manager.DeleteTask(deleteIndex - 1))
					Console.WriteLine("Task deleted");
				else
					Console.WriteLine("Invalid Index");
			}
			else
				Console.WriteLine("Invalid Number");
		}

		private void ModifyTaskFromInput()
		{
			Console.WriteLine("Task index to modify : ");
			manager.ShowTasks();
			string? toModifyIndex = Console.ReadLine();
			if (int.TryParse(toModifyIndex, out int editIndex))
			{
				Console.WriteLine("Enter new description :");
				string? newDescription = Console.ReadLine();
				if (!string.IsNullOrWhiteSpace(newDescription))
				{
					if (manager.ModifyTask(editIndex - 1, newDescription))
						Console.WriteLine("Task modified");
					else
						Console.WriteLine("Couldnt modify task");
				}
			}
			else
				Console.WriteLine("Invalid Index");
		}

		public void HandleCommand(string input)
		{
			switch (input)
			{
				case "ADD":
					AddTaskFromInput();
					break;

				case "DELETE":
					DeleteTaskFromInput();
					break;

				case "SHOW":
					manager.ShowTasks();
					break;

				case "MODIFY":
					ModifyTaskFromInput();
					break;

				default:
					Console.WriteLine("Unknown Command");
					break;
			}
		}
	}
}

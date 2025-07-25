using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO_list_manager
{
	internal class TaskManager
	{
		private readonly List<Task> tasks;

		public TaskManager()
		{
			tasks = [];
		}

		public void AddTask(string description)
		{
			tasks.Add(new Task(description));
		}

		public bool DeleteTask(int index)
		{
			if (index >= 0 && index < tasks.Count)
			{
				tasks.RemoveAt(index);
				return true;
			}
			return false;
		}

		public void ShowTasks()
		{
			int it = 0;
			while (it < tasks.Count)
			{
				Console.Write("-");
				Console.Write(tasks.ElementAt(it).Description);
				Console.Write(" at index: ");
				Console.Write(it + 1);
				if (it != tasks.Count - 1)
					Console.WriteLine(",");
				else
					Console.WriteLine();
				it++;
			}
		}

		public bool ModifyTask(int index, string newDescription)
		{
			if (index < tasks.Count && index >= 0)
			{
				tasks.ElementAt(index).Description = newDescription;
				return true;
			}
			return false;
		}
	}
}

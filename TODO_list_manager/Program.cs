
namespace TODO_list_manager
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("TASKMANAGER");
			Console.WriteLine("Usage: ADD | DELETE | SHOW | MODIFY");

			var inputHandler = new InputHandler();

			while (true)
			{
				string? input = Console.ReadLine();
				if (!string.IsNullOrWhiteSpace(input))
					inputHandler.HandleCommand(input);
			}
		}
	}
}
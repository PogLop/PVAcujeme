namespace Program;

public static class tui {
	public static string? Choose(string title, string[] options)
	{

		string? ret = null;
		int selector = 0;
		int longest = options.Max(o => o.Length);
		ConsoleKeyInfo ki = new ConsoleKeyInfo();
		
		do
		{

			while ((Console.BufferHeight < options.Length + 6) || (Console.BufferWidth < longest + 6))
			{
				//	print error
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("ERROR");
				Console.ResetColor();
				Console.WriteLine($"terminal too small ( got {Console.BufferWidth}x{Console.BufferHeight} needs {options.Length+6}x{longest + 6} )");
				
				Thread.Sleep(100);
			}
			
			Console.Clear();
			
			//	print title
			Console.ForegroundColor = ConsoleColor.Green;
			Console.SetCursorPosition((Console.BufferWidth/2) - (title.Length/2), Console.CursorTop + 1);
			Console.Write(title);
			Console.ResetColor();
			
			
			//	print options
			for (int i = 0; i < options.Length; i++)
			{
				Console.SetCursorPosition((Console.BufferWidth/2) - (options[i].Length/2), 3 + i);
				if (selector == i)
				{
					Console.WriteLine(options[i]);
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine(options[i]);
					Console.ResetColor();
				}
			}
			
			//	process user input
			ki = Console.ReadKey(false);

			switch (ki.Key)
			{
				case ConsoleKey.Enter:
				{
					ret = options[selector];
					break;
				}
				case ConsoleKey.Escape:
				{
					return null;
				}
				case ConsoleKey.UpArrow:
				{
					selector--;
					if (selector < 0)
					{
						selector = options.Length - 1;
					}
					break;
				}
				case ConsoleKey.DownArrow:
				{
					selector++;
					if (selector >= options.Length)
					{
						selector = 0;
					}
					break;
				}
			}


		} while (ret == null);

		return ret;
	}
}
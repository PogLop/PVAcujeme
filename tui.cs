using System.Diagnostics.CodeAnalysis;

namespace Program;
using System;

public static class Tui {
	
	public static bool IsPrintable(char c)
	{
		return c >= 32 && c <= 126;
	}
	
	//	Methods to be used in Terminal User Interface
	
	public static string? Choose(string title, ref string[] options)
	{
		///	necha uzivatele vybrat, co chce delat
		/// vrati options[selected] jako string

		string? ret = null;
		int selector = 0;
		int longest = options.Max(o => o.Length);
		ConsoleKeyInfo ki = new ConsoleKeyInfo();
		
		do
		{
			
			Console.ResetColor();

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

	public enum FormFieldType
	{
		String,
		Int,
		Double,
		Boolean,
	}
	
	public struct FormField
	{

		public FormField(string name, FormFieldType type)
		{
			this.Value = null;
			this.Name = new string(name);
			this.Type = type;
		}


		public bool Err { get; set; }
		public FormFieldType Type { get; private set; }
		public string Name { get; private set; }
		public string? Value { get; set; }

		public int Len()
		{
			return this.Name.Length + 2 /*": "*/ + ((this.Value != null)? this.Value.Length : 0);
		}

		public bool Check()
		{
			if (this.Value == null)
			{
				return false;
			}

			switch (this.Type)
			{
				case FormFieldType.String:
				{
					return true;
				}
				case FormFieldType.Int:
				{
					try
					{
						Convert.ToInt64(this.Value);
					}
					catch (Exception)
					{
						return false;
					}

					return true;
				}
				case FormFieldType.Double:
				{
					try
					{
						Convert.ToDouble(this.Value);
					}
					catch (Exception)
					{
						return false;
					}
					return true;
				}
				case FormFieldType.Boolean:
				{
					string val = this.Value.ToLower();
					return val == "yes" || val == "no";
				}
				default:
				{
					return false;
				}
			}
		}
		
		public T? Into<T>()
		{

			if (this.Value == null)
			{
				throw new NullReferenceException();
			}
			if (typeof(T) == typeof(string))
			{
				if (this.Type == FormFieldType.String)
				{
					return (T)((object)this.Value);
				}
			}
			else if (typeof(T) == typeof(int))
			{
				if (this.Type == FormFieldType.Int)
				{
					return (T)((object)System.Convert.ToInt64(this.Value));
				}
			}
			else if (typeof(T) == typeof(double))
			{
				if (this.Type == FormFieldType.Double)
				{
					return (T)((object)System.Convert.ToDouble(this.Value));
				}
			}
			else if (typeof(T) == typeof(bool))
			{
				if (this.Type == FormFieldType.Boolean)
				{
					return (T)((object)System.Convert.ToBoolean(this.Value));
				}
			}
			
			
			return (T?)((object?)null);
		}

	}


	public static bool Form(string title, ref FormField[] options)
	{
		///	necha uzivatele vlozit data do FormField a vrati bool jestli vlozeni bylo uspesne
		/// kdyz vraci false, nastala chyba na strane uzivatele
		
		string done = new string("done");
		string exit = new string("exit");

		
		int selector = 0;
		int longest = options.Max(o => o.Len());
		ConsoleKeyInfo ki = new ConsoleKeyInfo();

		do
		{
			
			Console.ResetColor();

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
			for (int i = 0; i < options.Length + 2; i++)
			{
				Console.ResetColor();
				if (selector == i)
				{
					Console.ForegroundColor = ConsoleColor.White;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.DarkGray;
				}
				if (i < options.Length)
				{
					if ((options[i].Err) && (selector != i))
					{
						Console.ForegroundColor = ConsoleColor.Red;
					}
					Console.SetCursorPosition((Console.BufferWidth / 2) - (options[i].Len() / 2), 3 + i);

					Console.WriteLine($"{options[i].Name}: {options[i].Value} | {options[i].Err}");
				}
				else
				{
					if (i == options.Length)
					{
						Console.SetCursorPosition((Console.BufferWidth/2) - (done.Length/2), 3 + i);
						Console.WriteLine(done);
					}
					else
					{
						Console.SetCursorPosition((Console.BufferWidth/2) - (exit.Length/2), 3 + i);
						Console.WriteLine(exit);
					}
				}
			}
			
			ki = Console.ReadKey(false);

			switch (ki.Key)
			{
				case ConsoleKey.Enter:
				{
					if (selector < options.Length)
					{
						ref var opt = ref options[selector];
						if (opt.Type == FormFieldType.Boolean)
						{
							if (opt.Value == null || opt.Value != "no")
							{
								opt.Value = new string("no");
							}
							else
							{
								opt.Value = new string("yes");
							}
						} else {

							if (selector < options.Length) {
								options[selector].Err = !options[selector].Check();
							}
							
							selector++;
							if (selector >= options.Length + 2) {
								selector = 0;
							}
						}
					} else {
						if (selector == options.Length) {
							foreach (var i in options) {
								if (i.Err) {
									return false;
								}
							}

							return true;
						}

						return false;
					}
					break;
				}
				case ConsoleKey.DownArrow: {
					if (selector < options.Length) {
						options[selector].Err = !options[selector].Check();
					}
					selector++;
					if (selector >= options.Length + 2) {
						selector = 0;
					}

					break;
				}
				case ConsoleKey.UpArrow: {
					if (selector < options.Length) {
						options[selector].Err = !options[selector].Check();
					}

					selector--;
					if (selector < 0) {
						selector = options.Length + 1;
					}
					break;
				}
				default: {
					if (selector >= options.Length) {
						break;
					}

					ref var opt = ref options[selector];
					if (opt.Type == FormFieldType.Boolean) {
						break;
					}

					if (opt.Value == null) {
						opt.Value = new string("");
					}

					if (ki.Key == ConsoleKey.Backspace) {
						if (opt.Value.Length == 0) {
							opt.Value = null;
						} else {
							opt.Value = opt.Value.Substring(0, opt.Value.Length - 1);
						}
					} else {
						if (IsPrintable(ki.KeyChar)) {
							opt.Value += ki.KeyChar;
						}
					}
					break;
				}
			}

			
		} while (true);
		

		return true;
	}
	
	
}
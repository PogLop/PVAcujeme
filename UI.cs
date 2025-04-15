using System;

namespace UI
{
    class Stuff
    {
        static int readMessages = 0;
        static int unreadMessages = 0;
        static int contactCount = 0;

        public static event EventHandler MessageRead;
        public static event EventHandler NewMessageReceived;
        public static event EventHandler ContactAdded;
        
        static Timer clockTimer;

        static void Main(string[] args)
        {
            MessageRead += OnMessageRead;
            NewMessageReceived += OnNewMessageReceived;
            ContactAdded += OnContactAdded;
            
            clockTimer = new Timer(1000);
            clockTimer.Elapsed += (s, e) => RenderStatusBar();
            clockTimer.Start();
            
            Console.WriteLine("Klávesy:\n[M] Nová zpráva\n[R] Přečíst zprávu\n[C] Přidat kontakt\n[Q] Konec\n");
            
            while (true)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.M:
                        NewMessageReceived?.Invoke(null, EventArgs.Empty);
                        break;
                    case ConsoleKey.R:
                        MessageRead?.Invoke(null, EventArgs.Empty);
                        break;
                    case ConsoleKey.C:
                        ContactAdded?.Invoke(null, EventArgs.Empty);
                        break;
                    case ConsoleKey.Q:
                        return;
                }
            }
        }
        
        static void OnMessageRead(object sender, EventArgs e)
        {
            if (unreadMessages > 0)
            {
                readMessages++;
                unreadMessages--;
            }
            RenderStatusBar();
        }

        static void OnNewMessageReceived(object sender, EventArgs e)
        {
            unreadMessages++;
            RenderStatusBar();
        }

        static void OnContactAdded(object sender, EventArgs e)
        {
            contactCount++;
            RenderStatusBar();
        }

        // zobrazení stavového radku
        static void RenderStatusBar()
        {
            int currentLineCursor = Console.CursorTop;

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(
                $" Zprávy: {readMessages}/{readMessages + unreadMessages} | " +
                $"Čas: {DateTime.Now:dd.MM.yyyy HH:mm:ss} | " +
                $"Kontaktů: {contactCount} "
                .PadRight(Console.WindowWidth - 1)
            );

            Console.ResetColor();
            Console.SetCursorPosition(0, currentLineCursor); 
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
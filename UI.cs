using System;
using System.Timers;

namespace UI
{
    public class Stuff
    {
        static int readMessages = 0;
        static int unreadMessages = 0;
        static int contactCount = 0;

        public static event EventHandler MessageRead;
        public static event EventHandler NewMessageReceived;
        public static event EventHandler ContactAdded;

        static System.Timers.Timer clockTimer;

        public static void Init()
        {
            MessageRead += OnMessageRead;
            NewMessageReceived += OnNewMessageReceived;
            ContactAdded += OnContactAdded;

            clockTimer = new System.Timers.Timer(1000);
            clockTimer.Elapsed += (s, e) => RenderStatusBar();
            clockTimer.Start();
        }

        public static void ProcessKey(ConsoleKey key)
        {
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
}

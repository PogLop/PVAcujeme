using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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

        private static Dictionary<string, string> contactDirectory = new();
        private static string loggedInUser;
        private static string lastMessagePreview = string.Empty;

        public static void Init()
        {
            MessageRead += OnMessageRead;
            NewMessageReceived += OnNewMessageReceived;
            ContactAdded += OnContactAdded;
        }

        public static bool Login(string username, string password)
        {
            string storedSalt = "staticSalt123"; // Ukázkově statická sůl, v reálu načíst per-user
            string storedHash = ComputeHash("password123", storedSalt); // Příklad uloženého hashe

            string inputHash = ComputeHash(password, storedSalt);
            if (inputHash == storedHash)
            {
                loggedInUser = username;
                return true;
            }
            return false;
        }

        public static void ImportContacts(string filePath)
        {
            if (!File.Exists(filePath)) return;
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && !contactDirectory.ContainsKey(parts[0]))
                {
                    contactDirectory.Add(parts[0], parts[1]);
                    ContactAdded?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        public static void SendMessage(string recipient, string encryptedMessage)
        {
            if (!contactDirectory.ContainsKey(recipient))
            {
                Console.WriteLine($"Kontakt {recipient} nenalezen.");
                return;
            }

            ReceiveMessage(recipient, encryptedMessage); // Simulace doručení zpět
        }

        public static void ReceiveMessage(string sender, string encryptedMessage)
        {
            lastMessagePreview = encryptedMessage;
            unreadMessages++;
            NewMessageReceived?.Invoke(null, EventArgs.Empty);
        }

        public static void ProcessKey(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.M:
                    Console.Write("Komu chcete poslat zprávu? ");
                    string recipient = Console.ReadLine();
                    Console.Write("Zadejte text zprávy: ");
                    string text = Console.ReadLine();
                    string encrypted = "[ZAŠIFROVANÁ ZPRÁVA: " + text + "]"; // simulace
                    SendMessage(recipient, encrypted);
                    break;
                case ConsoleKey.R:
                    MessageRead?.Invoke(null, EventArgs.Empty);
                    break;
                case ConsoleKey.C:
                    Console.Write("Zadejte jméno kontaktu: ");
                    string name = Console.ReadLine();
                    Console.Write("Zadejte veřejný klíč: ");
                    string keyStr = Console.ReadLine();
                    if (!contactDirectory.ContainsKey(name))
                    {
                        contactDirectory.Add(name, keyStr);
                        ContactAdded?.Invoke(null, EventArgs.Empty);
                    }
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
            RenderStatusBar();
        }

        static void OnContactAdded(object sender, EventArgs e)
        {
            contactCount++;
            RenderStatusBar();
        }

        static void RenderStatusBar()
        {
            Console.Clear();
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write((" Poslední zpráva: " + lastMessagePreview).PadRight(Console.WindowWidth - 1));

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(
                ($" Nepřečtené: {unreadMessages} | " +
                $"Čas: {DateTime.Now:dd.MM.yyyy HH:mm:ss} | " +
                $"Kontaktů: {contactCount} " +
                " | Klávesy: [M] Zpráva [R] Ozančit jako přečtené [C] Kontakt [Q] Konec\n").PadRight(Console.WindowWidth - 1)
            );

            Console.ResetColor();
        }

        private static string ComputeHash(string input, string salt)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input + salt);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}

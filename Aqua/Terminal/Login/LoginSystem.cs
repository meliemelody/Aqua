using System;
using static Aqua.Kernel;
using term = Aqua.Terminal.Terminal;
using Aqua.Terminal.Accounts;

namespace Aqua.Terminal.Login
{
    public class LoginSystem
    {
        public static String username, password;
        public static bool needsPassword;

        public static void Start()
        {
            // Introduction
            String welcome = "Welcome to the world of Aqua.";

            // Center the cursor position
            Console.SetCursorPosition((Console.WindowWidth / 2) - (welcome.Length / 2) - 2, Console.CursorTop);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(welcome);

            String login = "Please login using your account information.";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition((Console.WindowWidth / 2) - (login.Length / 2) - 2, Console.CursorTop);

            Console.WriteLine(login + "\n");

            // Console.WriteLine("Input your username : ");
            /*GetUsername();

            if (needsPassword)
                GetPassword();*/

            Login();
        }

        public static void Login()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Input your username : ");

                Console.ForegroundColor = ConsoleColor.White;
                var username = Console.ReadLine();

                Account found_user = null;
                foreach (var user in Kernel.accountManager.accounts)
                {
                    if (user.GetUsername() == username)
                    {
                        found_user = user;
                    }
                }

                if (found_user == null)
                {
                    term.DebugWrite($"Username {username} does not exist!", 4);
                    continue;
                }

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("Input your password : ");

                    Console.ForegroundColor = ConsoleColor.White;
                    string password = "";
                    ConsoleKeyInfo key_info;

                    do
                    {
                        key_info = Console.ReadKey(true);

                        if (key_info.Key != ConsoleKey.Backspace && key_info.Key != ConsoleKey.Enter)
                        {
                            password += key_info.KeyChar;
                            Console.Write("*");
                        }
                        else
                        {
                            if (key_info.Key == ConsoleKey.Backspace && password.Length > 0)
                            {
                                password = password.Substring(0, (password.Length - 1));
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                                Console.Write(" ");
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            }
                        }
                    }
                    while (key_info.Key != ConsoleKey.Enter);
                    Console.Write("\n");

                    if (found_user.VerifyPassword(password))
                    {
                        break;
                    }

                    Console.WriteLine("Invalid password.");
                }

                curAccount = found_user;
                break;
            }
            term.DebugWrite("Successfully logged in. Welcome " + username + ".", 2);

            Cosmos.HAL.Global.PIT.Wait(1000);
            StartUp();
        }
    }
}

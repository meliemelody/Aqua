using Aqua.Miscellaneous;
using System;
using System.IO;
using static Aqua.Kernel;
using futils = Aqua.Filesystem.Utilities;
using term = Aqua.Terminal.Terminal;

namespace Aqua.Terminal.Accounts
{
    public class LoginSystem
    {
        public static string username, password;
        static int count = 0;

        /// <summary>
        /// It checks if the folder exists, if it doesn't, it creates it
        /// </summary>
        public static void Start()
        {
            FolderCheck();

            // Introduction
            string welcome = "Welcome to the world of Aqua.";

            // Center the cursor position
            Console.SetCursorPosition((Console.WindowWidth / 2) - (welcome.Length / 2) - 2, Console.CursorTop);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(welcome + "\n");

            LogString();

            if (!File.Exists(@"0:\AquaSys\Login\Username.acf") || !File.Exists(@"0:\AquaSys\Login\Password.acf"))
                SetUsername();
            else
                GetUsername();
        }

        /// <summary>
        /// It writes a string to the console
        /// </summary>
        private static void LogString()
        {
            String logString;

            if (!File.Exists(@"0:\AquaSys\Login\Username.acf") || !File.Exists(@"0:\AquaSys\Login\Password.acf"))
                logString = "Please create an account.";
            else
                logString = "Please input your account information.";

            Console.SetCursorPosition((Console.WindowWidth / 2) - (logString.Length / 2) - 2, Console.CursorTop);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(logString);
        }

        /// <summary>
        /// If the directory doesn't exist, create it
        /// </summary>
        private static void FolderCheck()
        {
            if (!Directory.Exists(@"0:\AquaSys"))
            {
                Directory.CreateDirectory(@"0:\AquaSys");
                // Console.WriteLine("ok for system");
            }

            if (!Directory.Exists(@"0:\AquaSys\Login"))
            {
                Directory.CreateDirectory(@"0:\AquaSys\Login");
                // Console.WriteLine("ok for login");
            }
        }

        public static void SetUsername()
        {
            // Just in case.
            FolderCheck();

            // Check for the user's input.
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nInput a username : ");

            Console.ForegroundColor = ConsoleColor.White;

            String input = Console.ReadLine();

            if (input != "guest")
            {
                File.WriteAllText(@"0:\AquaSys\Login\Username.acf", Decryption.Encrypt(input));
                username = input;
                // SetPassword();

                Cosmos.HAL.Global.PIT.Wait(250);
                SetPassword();
            }
            else
            {
                term.DebugWrite("Username already used in system.", 4);

                Cosmos.HAL.Global.PIT.Wait(250);
                SetUsername();
            }
        }

        /// <summary>
        /// It creates a file called Password.acf and writes the encrypted password to it
        /// </summary>
        public static void SetPassword()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nInput a password : ");

            Console.ForegroundColor = ConsoleColor.Black;

            String input = Console.ReadLine();

            File.Create(@"0:\AquaSys\Login\Password.acf");
            File.WriteAllText(@"0:\AquaSys\Login\Password.acf", Decryption.Encrypt(input));
            // SetPassword();

            SetRoot();
        }

        /// <summary>
        /// It asks the user if they want to be root, and if they do, it writes "true" to a file, and if
        /// they don't, it writes "false" to a file
        /// </summary>
        private static void SetRoot()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nDo you want to be root [administrator] ? [y/n] : ");

            Console.ForegroundColor = ConsoleColor.White;
            ConsoleKeyInfo input = Console.ReadKey();
               
            if (input.Key == ConsoleKey.Y)
                File.WriteAllText(@"0:\AquaSys\Login\Root.acf", "true");
            else if (input.Key == ConsoleKey.N)
                File.WriteAllText(@"0:\AquaSys\Login\Root.acf", "false");
            else
            {
                term.DebugWrite("The input was not recognized, try again.", 4);
                SetRoot();
            }

            Console.WriteLine();
            term.DebugWrite("Successfully created the account : \"" + username + "\".\n", 2);

            Cosmos.HAL.Global.PIT.Wait(1250);
            Console.Clear();
            LogString();

            GetUsername();
        }

        private static string GetUsername()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nInput your username : ");

            Console.ForegroundColor = ConsoleColor.White;

            String input = Console.ReadLine();
            username = Decryption.Decrypt(futils.ReadLine(@"0:\AquaSys\Login\Username.acf", 0));

            var rootCheck = futils.ReadLine(@"0:\AquaSys\Login\Root.acf", 0);

            if (input == username)
            {
                if (rootCheck == "true")
                    Kernel.isRoot = true;
                else
                    Kernel.isRoot = false;

                GetPassword();
                return input;
            }
            else if (input == "guest")
            {
                username = "guest";
                Kernel.isRoot = false;

                Login();
                return input;
            }
            else
            {
                term.DebugWrite("Invalid username, try again.\n", 4);
                Cosmos.HAL.Global.PIT.Wait(250);

                GetUsername();
            }

            return null;
        }

        public static string GetPassword()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nInput your password : ");

            Console.ForegroundColor = ConsoleColor.Black;

            String input = Console.ReadLine();
            password = Decryption.Decrypt(futils.ReadLine(@"0:\AquaSys\Login\Password.acf", 0));

            if (input == password)
                Login();
            else
            {
                // After 3 attempts, it will redirect you to the username page again. 
                if (count == 2)
                {
                    term.DebugWrite("3 incorrect attempts have been made, please input yours or another username.\n", 4);

                    count = 0;
                    GetUsername();
                }
                else
                {
                    term.DebugWrite("Invalid password, try again.\n", 4);

                    count++;
                    GetPassword();
                }

                Cosmos.HAL.Global.PIT.Wait(250);
            }

            // --- DEBUG ---
            // Print the count variable's value.
            // Console.WriteLine($"Attempt count : {count}");

            Cosmos.HAL.Global.PIT.Wait(500);
            return null;
        }

        public static void Login()
        {
            term.DebugWrite("Successfully logged in. Welcome " + username + ".\n", 2);

            Cosmos.HAL.Global.PIT.Wait(750);
            //Console.Clear();
            StartUp();
        }
    }
}

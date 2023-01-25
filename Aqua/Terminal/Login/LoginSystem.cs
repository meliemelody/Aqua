using Aqua.Commands.Executables;
using System;
using System.IO;
using static Aqua.Kernel;
using futils = Aqua.Filesystem.FileUtilities;
using term = Aqua.Terminal.Terminal;

namespace Aqua.Terminal.Login
{
    public class LoginSystem
    {
        public static String username, password;

        public static void Start()
        {
            FolderCheck();

            // Introduction
            String welcome = "Welcome to the world of Aqua.";

            // Center the cursor position
            Console.SetCursorPosition((Console.WindowWidth / 2) - (welcome.Length / 2) - 2, Console.CursorTop);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(welcome + "\n");

            LogString();

            if (!File.Exists(@"0:\System\Login\Username.cfg") || !File.Exists(@"0:\System\Login\Password.cfg"))
                SetUsername();
            else
                GetUsername();
        }

        private static void LogString()
        {
            String logString;

            if (!File.Exists(@"0:\System\Login\Username.cfg") || !File.Exists(@"0:\System\Login\Password.cfg"))
                logString = "Please create an account.";
            else
                logString = "Please input your account information.";

            Console.SetCursorPosition((Console.WindowWidth / 2) - (logString.Length / 2) - 2, Console.CursorTop);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(logString);
        }

        private static void FolderCheck()
        {
            if (!Directory.Exists(@"0:\System"))
            {
                Directory.CreateDirectory(@"0:\System");
            }

            if (!Directory.Exists(@"0:\System\Login"))
            {
                Directory.CreateDirectory(@"0:\System\Login");
            }
        }

        public static void SetUsername()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nInput a username : ");

            Console.ForegroundColor = ConsoleColor.White;

            String input = Console.ReadLine();

            if (input != "system" || input != "guest")
            {
                File.WriteAllText(@"0:\System\Login\Username.cfg", input);
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

        public static void SetPassword()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nInput a password : ");

            Console.ForegroundColor = ConsoleColor.Black;

            String input = Console.ReadLine();

            if (input != null || input != "")
            {
                File.WriteAllText(@"0:\System\Login\Password.cfg", input);
                // SetPassword();

                term.DebugWrite("\nSuccessfully created the account : \"" + username + "\".\n", 4);

                Cosmos.HAL.Global.PIT.Wait(250);
                Console.Clear();
                LogString();

                GetUsername();
            }
            else
            {
                term.DebugWrite("Password must not be empty.\n", 4);

                Cosmos.HAL.Global.PIT.Wait(250);
                SetPassword();
            }
        }

        private static String GetUsername()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nInput your username : ");

            Console.ForegroundColor = ConsoleColor.White;

            String input = Console.ReadLine();
            username = futils.ReadLine(@"0:\System\Login\Username.cfg", 0);

            if (input == username)
            {
                Kernel.isRoot = true;

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

        public static String GetPassword()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nInput your password : ");

            Console.ForegroundColor = ConsoleColor.Black;

            String input = Console.ReadLine();
            password = futils.ReadLine(@"0:\System\Login\Password.cfg", 0);

            if (input == password)
                Login();
            else
            {
                term.DebugWrite("Invalid password, try again.\n", 4);
                Cosmos.HAL.Global.PIT.Wait(250);

                GetPassword();
            }


            Cosmos.HAL.Global.PIT.Wait(500);

            return null;
        }

        public static void Login()
        {
            Console.WriteLine(' ');
            term.DebugWrite("Successfully logged in. Welcome " + username + ".\n", 2);

            Cosmos.HAL.Global.PIT.Wait(750);
            //Console.Clear();
            StartUp();
        }
    }
}

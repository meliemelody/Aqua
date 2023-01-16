using System;
using static Aqua.Kernel;
using term = Aqua.Terminal.Terminal;

namespace Aqua.Terminal.Login
{
    public class LoginSystem
    {
        public static String username, password;

        public static void Start()
        {
            // Console.WriteLine("Input your username : ");
            GetUsername();
            GetPassword();
            Login();
        }

        public static String GetUsername()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nInput your username : ");
            
            Console.ForegroundColor = ConsoleColor.White;

            String input = Console.ReadLine();
            if (input != null)
            {
                Cosmos.HAL.Global.PIT.Wait(500);
                switch (input)
                {
                    case "root":
                        isRoot = true;
                        username = "root";
                        return input;

                    case "user":
                        isRoot = false;
                        username = "user";
                        return input;

                    default:
                        term.DebugWrite("Invalid username, try again.\n", 4);
                        Cosmos.HAL.Global.PIT.Wait(250);

                        GetUsername();
                        break;
                }

                Cosmos.HAL.Global.PIT.Wait(250);
            }

            return null;
        }

        public static String GetPassword()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nInput your password : ");

            Console.ForegroundColor = ConsoleColor.Black;

            String input = Console.ReadLine();
            Cosmos.HAL.Global.PIT.Wait(500);
            if (username == "root")
            {
                if (input == "root")
                {
                    password = input;
                    return input;
                } 
                else
                {
                    term.DebugWrite("Invalid password, try again.\n", 4);
                    Cosmos.HAL.Global.PIT.Wait(500);

                    GetPassword();
                    return null;
                }
            }
            else if (username == "user")
            {
                if (input == "" || input == null)
                {
                    password = input;
                    return input;
                }
                else
                {
                    term.DebugWrite("Invalid password, try again.\n", 4);
                    Cosmos.HAL.Global.PIT.Wait(500);

                    GetPassword();
                    return null;
                }
            }

            Cosmos.HAL.Global.PIT.Wait(500);
            username = input;

            return null;
        }

        public static void Login()
        {
            term.DebugWrite("Successfully logged in. Welcome " + username + ".", 2);

            Cosmos.HAL.Global.PIT.Wait(1000);
            Console.Clear();

            StartUp();
        }
    }
}

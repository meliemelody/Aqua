using System;

namespace Aqua.Terminal
{
    public class Terminal
    {
        public Terminal() { }

        public static String DebugWrite(String msg, int mode)
        {
            Console.Write("  ");

            switch (mode)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("[Debug] ");
                    break;

                case 1:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("[Kernel] ");
                    break;

                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[Success] ");
                    break;

                case 3:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("[Warning] ");
                    break;

                case 4:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[Error] ");
                    break;

                case 5:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("[Critical] ");
                    break;

                case 6:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("[Information] ");
                    break;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(msg);

            return "";
        }
    }
}

using System;

namespace Aqua.Terminal
{
    public class Terminal
    {
        public Terminal() { }

        /// <summary>
        /// It writes a message to the console with a timestamp and a prefix
        /// </summary>
        /// <param name="msg">The message you want to display</param>
        /// <param name="mode">0 = Debug, 1 = Kernel, 2 = Success, 3 = Warning, 4 = Error, 5 = Critical,
        /// 6 = Information</param>
        /// <returns>
        /// A string.
        /// </returns>
        public static String DebugWrite(String msg, int mode)
        {
            Console.Write("  ");

            switch (mode)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("[Debug] | " + $"{Cosmos.HAL.RTC.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")}:{Cosmos.HAL.RTC.Second.ToString("D2")} | ");
                    break;

                case 1:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("[Kernel] | " + $"{Cosmos.HAL.RTC.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")}:{Cosmos.HAL.RTC.Second.ToString("D2")} | ");
                    break;

                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[Success] | " + $"{Cosmos.HAL.RTC.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")}:{Cosmos.HAL.RTC.Second.ToString("D2")} | ");
                    break;

                case 3:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("[Warning] | " + $"{Cosmos.HAL.RTC.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")}:{Cosmos.HAL.RTC.Second.ToString("D2")} | ");
                    break;

                case 4:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[Error] | " + $"{Cosmos.HAL.RTC.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")}:{Cosmos.HAL.RTC.Second.ToString("D2")} | ");
                    break;

                case 5:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("[Critical] | " + $"{Cosmos.HAL.RTC.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")}:{Cosmos.HAL.RTC.Second.ToString("D2")} | ");
                    break;

                case 6:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("[Information] | " + $"{Cosmos.HAL.RTC.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")}:{Cosmos.HAL.RTC.Second.ToString("D2")} | ");
                    break;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(msg);

            return null;
        }
    }
}

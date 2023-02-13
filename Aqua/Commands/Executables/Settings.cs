using Aqua.Terminal;
using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Commands.Executables
{
    public class Set : Command
    {
        public Set(string name, string description) : base(name, description) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "bg":
                    switch (args[1])
                    {
                        case "blue":
                            Console.BackgroundColor = ConsoleColor.Blue;
                            break;

                        case "green":
                            Console.BackgroundColor = ConsoleColor.Green;
                            break;

                        case "red":
                            Console.BackgroundColor = ConsoleColor.Red;
                            break;

                        case "yellow":
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            break;

                        case "black":
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;

                        case "white":
                            Console.BackgroundColor = ConsoleColor.White;
                            break;

                        case "cyan":
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            break;

                        default:
                            return "Please select a correct color.";
                    }

                    return null;

                case "fg":
                    switch (args[1])
                    {
                        case "blue":
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;

                        case "green":
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;

                        case "red":
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;

                        case "yellow":
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;

                        case "black":
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;

                        case "white":
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                        case "cyan":
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;

                        default:
                            return Terminal.Terminal.DebugWrite("Please select a correct color.", 4);
                    }

                    return null;

                default:
                    return Terminal.Terminal.DebugWrite("Specify a correct argument.\n", 4);
            }
        }
    }

    public class Get : Command
    {
        public Get(string name, string description) : base(name, description) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "cpu":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    //return $"Vendor: {CPU.GetCPUVendorName()}\n  Name: {CPU.GetCPUBrandString()}\n  Frequency: {CPU.GetCPUCycleSpeed()}";
                    return "test";

                default:
                    return Terminal.Terminal.DebugWrite("Specify a correct argument.", 4);
            }
        }
    }
}

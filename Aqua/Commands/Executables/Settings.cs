using Cosmos.System;
using Cosmos.System.ScanMaps;
using System;
using Console = System.Console;

namespace Aqua.Commands.Executables
{
    public class Set : Command
    {
        public Set(string name, string description)
            : base(name, description) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "bg":
                    switch (args[1])
                    {
                        // Light colors
                        case "blue"
                        or "b":
                            Kernel.bgColor = ConsoleColor.Blue;
                            break;

                        case "red"
                        or "r":
                            Kernel.bgColor = ConsoleColor.Red;
                            break;

                        case "yellow"
                        or "y":
                            Kernel.bgColor = ConsoleColor.Yellow;
                            break;

                        case "green"
                        or "g":
                            Kernel.bgColor = ConsoleColor.Green;
                            break;

                        case "magenta"
                        or "m":
                            Kernel.bgColor = ConsoleColor.Magenta;
                            break;

                        case "cyan"
                        or "c":
                            Kernel.bgColor = ConsoleColor.Cyan;
                            break;

                        // Dark-toned colors
                        case "darkblue"
                        or "db":
                            Kernel.bgColor = ConsoleColor.DarkBlue;
                            break;

                        case "darkgreen"
                        or "dg":
                            Kernel.bgColor = ConsoleColor.DarkGreen;
                            break;

                        case "darkcyan"
                        or "dc":
                            Kernel.bgColor = ConsoleColor.DarkCyan;
                            break;

                        case "darkmagenta"
                        or "dm":
                            Kernel.bgColor = ConsoleColor.DarkMagenta;
                            break;

                        case "darkred"
                        or "dr":
                            Kernel.bgColor = ConsoleColor.DarkRed;
                            break;

                        case "darkyellow"
                        or "dy":
                            Kernel.bgColor = ConsoleColor.DarkYellow;
                            break;

                        // Monochrome colors
                        case "gray"
                        or "g":
                            Kernel.bgColor = ConsoleColor.Gray;
                            break;

                        case "darkgray"
                        or "dg":
                            Kernel.bgColor = ConsoleColor.DarkGray;
                            break;

                        case "black"
                        or "d":
                            Kernel.bgColor = ConsoleColor.Black;
                            break;

                        case "white"
                        or "w":
                            Kernel.bgColor = ConsoleColor.White;
                            break;

                        default:
                            return Terminal.Screen.DebugWrite("Please select a correct color.", 4);
                    }
                    Filesystem.Utilities.WriteLine(@"0:\AquaSys\Config\Colors.acf", args[1], false);
                    Console.Clear();
                    return null;

                case "fg":
                    switch (args[1])
                    {
                        // Light colors
                        case "blue"
                        or "b":
                            Kernel.fgColor = ConsoleColor.Blue;
                            break;

                        case "red"
                        or "r":
                            Kernel.fgColor = ConsoleColor.Red;
                            break;

                        case "yellow"
                        or "y":
                            Kernel.fgColor = ConsoleColor.Yellow;
                            break;

                        case "green"
                        or "g":
                            Kernel.fgColor = ConsoleColor.Green;
                            break;

                        case "magenta"
                        or "m":
                            Kernel.fgColor = ConsoleColor.Magenta;
                            break;

                        case "cyan"
                        or "c":
                            Kernel.fgColor = ConsoleColor.Cyan;
                            break;

                        // Dark-toned colors
                        case "darkblue"
                        or "db":
                            Kernel.fgColor = ConsoleColor.DarkBlue;
                            break;

                        case "darkgreen"
                        or "dg":
                            Kernel.fgColor = ConsoleColor.DarkGreen;
                            break;

                        case "darkcyan"
                        or "dc":
                            Kernel.fgColor = ConsoleColor.DarkCyan;
                            break;

                        case "darkmagenta"
                        or "dm":
                            Kernel.fgColor = ConsoleColor.DarkMagenta;
                            break;

                        case "darkred"
                        or "dr":
                            Kernel.fgColor = ConsoleColor.DarkRed;
                            break;

                        case "darkyellow"
                        or "dy":
                            Kernel.fgColor = ConsoleColor.DarkYellow;
                            break;

                        // Monochrome colors
                        case "gray"
                        or "g":
                            Kernel.fgColor = ConsoleColor.Gray;
                            break;

                        case "darkgray"
                        or "dg":
                            Kernel.fgColor = ConsoleColor.DarkGray;
                            break;

                        case "black"
                        or "d":
                            Kernel.fgColor = ConsoleColor.Black;
                            break;

                        case "white"
                        or "w":
                            Kernel.fgColor = ConsoleColor.White;
                            break;

                        default:
                            return Terminal.Screen.DebugWrite("Please select a correct color.", 4);
                    }
                    Filesystem.Utilities.WriteLine(@"0:\AquaSys\Config\Colors.acf", args[1], true);
                    return null;

                case "keymap":
                    try
                    {
                        switch (args[1])
                        {
                            case "us":
                                KeyboardManager.SetKeyLayout(new USStandardLayout());
                                break;

                            case "fr":
                                KeyboardManager.SetKeyLayout(new FRStandardLayout());
                                break;

                            case "de":
                                KeyboardManager.SetKeyLayout(new DEStandardLayout());
                                break;

                            case "gb":
                                KeyboardManager.SetKeyLayout(new GBStandardLayout());
                                break;

                            case "es":
                                KeyboardManager.SetKeyLayout(new ESStandardLayout());
                                break;

                            case "tr":
                                KeyboardManager.SetKeyLayout(new TRStandardLayout());
                                break;

                            default:
                                return Terminal.Screen.DebugWrite(
                                    "Please select a correct key mapping.",
                                    4
                                );
                        }
                        if (!System.IO.Directory.Exists(@"0:\AquaSys\Config"))
                            System.IO.Directory.CreateDirectory(@"0:\AquaSys\Config");

                        System.IO.File.WriteAllText(@"0:\AquaSys\Config\KeyMap.acf", args[1]);
                        return Terminal.Screen.DebugWrite(
                            $"Successfully set the keyboard to \"{args[1]}\".",
                            2
                        );
                    }
                    catch (Exception e)
                    {
                        return Terminal.Screen.DebugWrite(e.ToString(), 4);
                    }

                default:
                    return Terminal.Screen.DebugWrite("Specify a correct argument.\n", 4);
            }
        }
    }

    public class Get : Command
    {
        public Get(string name, string description)
            : base(name, description) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "ram":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    double ramUsage =
                        (double)(Cosmos.Core.GCImplementation.GetUsedRAM() / 1024 / 1024)
                        / (double)Cosmos.Core.CPU.GetAmountOfRAM();
                    return "RAM usage : " + ((int)ramUsage * 100).ToString() + "%";

                default:
                    return Terminal.Screen.DebugWrite("Specify a correct argument.", 4);
            }
        }
    }
}

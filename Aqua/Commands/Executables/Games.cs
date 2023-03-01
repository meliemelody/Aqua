using Aqua.Terminal;
using System;
using static System.Console;

namespace Aqua.Commands.Executables
{
    public class Games : Command
    {
        public Games(string name, string description) : base(name, description) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "maze":
                    Aqua.Games.Maze.Game();
                    return null;

                case "help":
                    ForegroundColor = ConsoleColor.DarkCyan;
                    WriteLine("  Available games :");

                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine("    - \"maze\"");
                    return null;

                default:
                    //return null;
                    return Terminal.Terminal.DebugWrite("Invalid argument, type \"help\" to list the available games.", 4);
            }
        }
    }
}

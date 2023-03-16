using Cosmos.System;
using System;
using System.Collections.Generic;
using term = Aqua.Terminal.Terminal;

namespace Aqua.Commands
{
    public class Manager
    {
        private readonly List<Command> _commands;
        public static List<string> commandStrings = new(), descriptionStrings = new();

        public Manager()
        {
            this._commands = new List<Command>()
            {
                // Executables
                new Executables.Print("prt", "Echoes back your sentence."),
                new Executables.Clear("clr",
                    "Clears your entire screen, and leaves you with only the shell and the bar"),
                new Executables.IO("io", "Used to shutdown or reboot/restart your computer."),
                new Executables.Time("time",
                    "Shows the current time with \"direct\", and the compile time with \"compile\"."),
                new Executables.Calculate("calc", "Calculate values between them [only 2 values allowed for now]"),
                new Executables.TextEditor("ted", "Execute the TED Editor, an all new text editor for Aqua System."),
                new Executables.Games("games", "Play some games and have fun!"),

                // Important commands
                new Executables.Get("get", "Get all of your system information, like your CPU or free memory."),
                new Executables.Set("set", "Set all of your settings, like background or foreground color."),

                // Filesystem commands
                new Filesystem.File("f",
                    "The file utilities, everything you need for creating, deleting and editing files."),
                new Filesystem.Directory("d",
                    "The directory utilities, same as the file utilities but with directories."),
                new Filesystem.Filesystem("fs",
                    "The file system utilities, used to format or see the type of the drive."),

                // System-wise commands
                new Terminal.Accounts.Accounts("acc", "Manage your accounts and log out."),
                new Network.Commands("net", "Manages your network and FTP."),
                new Network.PackageManager("pm", "The general package manager for Aqua."),
                new Executables.Manual("man", "List all the commands and their description."),
                new Interpreter.LangCommand("lang", "Execute the programs made in AquaLang."),
                new Executables.Tabbing("tab", "Manage all your tab fantasies.")
            };

            foreach (Command command in this._commands)
            {
                commandStrings.Add(command.name);
                descriptionStrings.Add(command.description);
            }
        }

        public String ProcessInput(String input)
        {
            String[] split = input.Split(' ');

            String label = split[0];
            List<String> args = new List<String>();

            if (label == "")
                return term.DebugWrite("Please input a correct command.", 4);

            int ctr = 0;
            foreach (String arg in split)
            {
                if (ctr != 0)
                    args.Add(arg);

                ctr++;
            }

            foreach (Command cmd in this._commands)
            {
                if (cmd.name == label)
                    return cmd.Execute(args.ToArray());
            }

            return term.DebugWrite("The command \"" + label + "\" does not exist.", 4);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using term = Aqua.Terminal.Terminal;

namespace Aqua.Commands
{
    public class Manager
    {
        private List<Command> commands;

        public Manager() 
        {
            this.commands = new List<Command>(1)
            {
                new Executables.Print("print"),
                new Executables.Clear("clear"),
                new Executables.IO("io"),

                new Aqua.Filesystem.File("f"),
                new Aqua.Filesystem.Directory("d"),
                new Aqua.Filesystem.Filesystem("fs"),

                new Aqua.Network.NetworkCommands("net")
            };
        }

        public String ProcessInput(String input)
        {
            String[] split = input.Split(' ');

            String label = split[0];
            List<String> args = new List<String>();

            int ctr = 0;
            foreach (String arg in split)
            {
                if (ctr != 0)
                    args.Add(arg);

                ctr++;
            }

            foreach (Command cmd in this.commands) 
            {
                if (cmd.name == label) 
                    return cmd.Execute(args.ToArray());
            }

            return term.DebugWrite("The command \"" + label + "\" does not exist.", 4);
        }
    }
}

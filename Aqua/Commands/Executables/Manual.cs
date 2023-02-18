using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Commands.Executables
{
    public class Manual : Command
    {
        public Manual (string name, string description) : base (name, description) { }

        public override string Execute(string[] args)
        {
            for (int i = 0; i < Manager.descriptionStrings.Count; i++)
            {
                if (Manager.commandStrings[i] == "f" || Manager.commandStrings[i] == "net" || Manager.commandStrings[i] == "get")
                    Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"   {Manager.commandStrings[i]} - ");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(Manager.descriptionStrings[i]);
            }

            return null;
        }
    }
}

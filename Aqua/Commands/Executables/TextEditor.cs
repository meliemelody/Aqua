using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Commands.Executables
{
    public class TextEditor : Command
    {
        public TextEditor(string name, string description) : base(name, description) { }

        public override string Execute(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("  Do you want to run the rewrote version of TED Editor ? [y/n] : ");

            Console.ForegroundColor = ConsoleColor.White;
            ConsoleKeyInfo input = Console.ReadKey();

            Console.WriteLine();
            if (input.Key == ConsoleKey.Y)
                return Miscellaneous.TEDEditor.Run(args);

            else if (input.Key == ConsoleKey.N)
                return Miscellaneous.Compatibility.TextEditor.Run(args);

            else
                return Terminal.Terminal.DebugWrite("Please input \"y\" or \"n\".", 4);
        }
    }
}

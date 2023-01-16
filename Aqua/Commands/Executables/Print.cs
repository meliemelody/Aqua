using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Commands.Executables
{
    public class Print : Command
    {
        public Print(string name) : base(name) { }

        public override string Execute(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            return "\"" + string.Join(' ', args) + "\"";
        }
    }
}

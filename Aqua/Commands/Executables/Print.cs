using System;

namespace Aqua.Commands.Executables
{
    public class Print : Command
    {
        public Print(string name, string description)
            : base(name, description) { }

        public override string Execute(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            return "\"" + string.Join(' ', args) + "\"";
        }
    }
}

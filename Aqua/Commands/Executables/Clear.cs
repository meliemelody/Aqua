using System;

namespace Aqua.Commands.Executables
{
    public class Clear : Command
    {
        public Clear(String name) : base(name) { }

        public override string Execute(string[] args)
        {
            Console.Clear();
            return "";
        }
    }
}

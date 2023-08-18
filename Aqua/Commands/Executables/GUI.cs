using Aqua.Interface;
using System;

namespace Aqua.Commands.Executables
{
    public class GUI : Command
    {
        public GUI(string name, string description)
            : base(name, description) { }

        public override string Execute(string[] args)
        {
            Base.Initialize();
            return null;
        }
    }
}

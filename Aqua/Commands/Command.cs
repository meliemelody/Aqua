using System;

namespace Aqua.Commands
{
    public class Command
    {
        public readonly String name;
        public Command(String name) { this.name = name; }

        public virtual String Execute(String[] args) { return ""; }
    }
}

using System;

namespace Aqua.Commands
{
    public class Command
    {
        public readonly String name;
        public readonly String description;

        public Command(String name)
        {
            this.name = name;
        }

        public Command(String name, String description)
        {
            this.name = name;
            this.description = description;
        }

        public virtual String Execute(String[] args)
        {
            return null;
        }
    }
}

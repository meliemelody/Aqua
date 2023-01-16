using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Commands
{
    public class Command
    {
        public readonly String name;
        public Command(String name) { this.name = name; }

        public virtual String Execute(String[] args) { return ""; }
    }
}

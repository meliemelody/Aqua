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
            return Miscellaneous.TextEditor.Run(args);
        }
    }
}

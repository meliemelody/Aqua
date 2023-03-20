using System;

namespace Aqua.Commands.Executables
{
    public class TextEditor : Command
    {
        public TextEditor(string name, string description)
            : base(name, description) { }

        public override string Execute(string[] args)
        {
            return Miscellaneous.TEDEditor.Run(args);
        }
    }
}

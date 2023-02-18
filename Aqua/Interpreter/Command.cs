using Aqua.Commands;

namespace Aqua.Interpreter
{
    public class LangCommand : Command
    {
        public LangCommand(string name, string description) : base(name, description) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "run":
                    if (System.IO.File.Exists(Kernel.currentDirectory + args[1]))
                        Language.Run(Kernel.currentDirectory + args[1]);
                    break;
            }

            return null;
        }
    }
}

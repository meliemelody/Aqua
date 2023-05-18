using Aqua.Commands;

namespace Aqua.Interpreter
{
    public class LangCommand : Command
    {
        public LangCommand(string name, string description)
            : base(name, description) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "run":
                    if (
                        System.IO.File.Exists(Kernel.currentDirectory + args[1])
                        && args[1].EndsWith(".alf")
                    )
                        Language.Run(Kernel.currentDirectory + args[1]);
                    else if (!args[1].EndsWith(".alf"))
                        return Terminal.Screen.DebugWrite("This file is not a .alf file.", 4);
                    else
                        return Terminal.Screen.DebugWrite("This file does not exist.", 4);

                    break;
            }

            return null;
        }
    }
}

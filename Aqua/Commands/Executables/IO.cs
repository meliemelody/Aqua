using System;
using term = Aqua.Terminal.Terminal;

namespace Aqua.Commands.Executables
{
    public class IO : Command
    {
        public IO (String name) : base (name) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            { 
                case "sd":
                    Cosmos.System.Power.Shutdown();
                    break;

                case "rb" or "rd":
                    Cosmos.System.Power.Reboot();
                    break;

                default:
                    return term.DebugWrite("Invalid argument.", 4);
            }

            return null;
        }
    }
}

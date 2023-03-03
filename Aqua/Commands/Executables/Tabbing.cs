using Aqua.Commands;
using System;

namespace Aqua.Commands.Executables
{
    public class Tabbing : Command
    {
        public Tabbing(string name, string description) : base(name, description) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "switch":
                    if (args[1] != null)
                    {
                        int.TryParse(args[1], out int result);
                        Tab.Change(result);
                    }
                    else
                        return Terminal.Terminal.DebugWrite("Please specify an index number.", 4);
                    break;

                case "new":
                    if (args[1] != null)
                        Tab.Add(args[1]);
                    else
                        Tab.Add("Untitled");
                    break;

                case "barvis":
                    Kernel.tabBarVisible = !Kernel.tabBarVisible;
                    Console.Clear();
                    break;

                default:
                    return Terminal.Terminal.DebugWrite("Invalid argument.", 4);
            }

            return null;
        }
    }
}

using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Commands.Executables
{
    public class System : Command
    {
        public System(string name, string description) : base(name, description) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "get":
                    switch (args[1])
                    {
                        case "cpu":
                            return $"Vendor: {CPU.GetCPUVendorName()}\n  Name: {CPU.GetCPUBrandString()}\n  Frequency: {CPU.GetCPUCycleSpeed()}";

                        default:
                            return Terminal.Terminal.DebugWrite("Specify a correct argument.", 4);
                    }

                default:
                    return Terminal.Terminal.DebugWrite("Specify a correct argument.", 4);
            }
        }
    }
}

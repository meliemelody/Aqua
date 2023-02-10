using Aqua.Commands;
using Aqua.Network;
using Aqua.Sounds;
using Cosmos.System;
using System;
using Console = System.Console;
using term = Aqua.Terminal.Terminal;

namespace Aqua.Terminal.Accounts
{
    public class Accounts : Command
    {
        public Accounts(string name, string description) : base(name, description) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "reset":
                    try
                    {
                        System.IO.Directory.Delete(@"0:\AquaSys\Login", true);
                        return term.DebugWrite("Deleted the login credentials, log out to make your new account.", 0);
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "out":
                    term.DebugWrite($"See you later, {LoginSystem.username}.", 0);
                    if (!VMTools.IsVMWare)
                        Sounds.Sounds.LogoffSound();
                    Network.Network.xClient.Close();
                    Cosmos.HAL.Global.PIT.Wait(2250);

                    Console.Clear();
                    LoginSystem.Start();
                    return null;

                default:
                    return term.DebugWrite("Invalid argument.", 4);
            }
        }
    }
}

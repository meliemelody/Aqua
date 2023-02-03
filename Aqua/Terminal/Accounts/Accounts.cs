﻿using Aqua.Commands;
using System;
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
                        System.IO.Directory.Delete(@"0:\System\Login");
                        return term.DebugWrite("Deleted the login credentials, log out to make your new account.", 0);
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "out":
                    term.DebugWrite("Goodbye " + LoginSystem.username + ", see you later !", 1);
                    Cosmos.HAL.Global.PIT.Wait(1000);

                    Console.Clear();
                    LoginSystem.Start();
                    return null;

                default:
                    return term.DebugWrite("Invalid argument.", 4);
            }
        }
    }
}
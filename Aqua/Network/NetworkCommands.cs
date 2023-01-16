using Aqua.Commands;
using Cosmos.System.Network.Config;
using CosmosFtpServer;
using System;
using term = Aqua.Terminal.Terminal;

namespace Aqua.Network
{
    public class NetworkCommands : Command
    {
        public NetworkCommands(String name) : base(name) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "ftp":
                    try
                    {
                        using (var xServer = new FtpServer(Kernel.fs, "0:\\"))
                        {
                            /** Listen for new FTP client connections **/
                            xServer.Listen();
                        }

                        return term.DebugWrite("FTP Server has been started.", 2);
                    }
                    catch (Exception ex)
                    {
                        return term.DebugWrite(ex.ToString(), 4);
                    }

                case "get":
                    if (args[1] == "ip")
                        return NetworkConfiguration.CurrentAddress.ToString();
                    else
                        return term.DebugWrite("Invalid argument.", 4);

                default:
                    return term.DebugWrite("Invalid argument.", 4);
            }
        }
    }
}

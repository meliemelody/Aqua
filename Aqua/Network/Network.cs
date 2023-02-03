using Aqua.Commands;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using CosmosFtpServer;
using System;
using System.Text;
using term = Aqua.Terminal.Terminal;

namespace Aqua.Network
{
    public class Network
    {
        public Network() { }

        public static void Setup()
        {
            using (var xClient = new DHCPClient())
            {
                /** Send a DHCP Discover packet **/
                //This will automatically set the IP config after DHCP response
                xClient.SendDiscoverPacket();
            }
        }
    }

    public class PackageManager : Command
    {
        public PackageManager(string name, string description) : base(name, description) { }

        public override string Execute(string[] args)
        {
            return Install(args[0], args[1], args[2], args[3]);
        }

        // Fork of Verde
        // This is in EARLY DEVELOPMENT, it will support package managing and getting later on.
        public static string Install(string address, string port, string message, string chs)
        {
            // Thank you Verde for the Package Manager code ! :]
            // This will be refactored in release 0.3

            /* This features a TCP connection
                network initialization is needed*/

            // Parse arguments
            Address add = Address.Parse(address);
            int destPort = Int32.Parse(port);

            // Base local port = 4242
            Console.WriteLine("Connection to destination host...");
            using var xClient = new TcpClient(destPort); // Ports should be corresponding
            xClient.Connect(add, destPort);

            // Send data
            Console.WriteLine("Sending request...");
            xClient.Send(Encoding.ASCII.GetBytes(message));

            // Receive data
            var endpoint = new EndPoint(Address.Zero, 0);
            Console.WriteLine("EndPoint set");

            var data = xClient.Receive(ref endpoint);  //set endpoint to remote machine IP:port
            Console.WriteLine("Received!");

            return Encoding.UTF8.GetString(data);
        }
    }

    public class Commands : Command
    {
        public Commands(String name, String description) : base(name, description) { }

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
                            term.DebugWrite($"FTP Server has been started at {NetworkConfiguration.CurrentAddress.ToString()}.", 2);
                            xServer.Listen();
                        }

                        return null;
                    }
                    catch (Exception ex)
                    {
                        return term.DebugWrite(ex.ToString(), 4);
                    }

                case "get":
                    if (args[1] == "ip")
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        return NetworkConfiguration.CurrentAddress.ToString();
                    }
                    else
                        return term.DebugWrite("Invalid argument.", 4);

                default:
                    return term.DebugWrite("Invalid argument.", 4);
            }
        }
    }
}

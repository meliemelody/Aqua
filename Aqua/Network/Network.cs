using Aqua.Commands;
using Cosmos.System.Network;
using System;
using System.Text;
using term = Aqua.Terminal.Screen;
using Cosmos.System.Network.IPv4.UDP.DNS;
using System.IO;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;

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
            try
            {
                /*WebClient wc = new WebClient(args[0]);
                Random random = new Random();
                string path = $"0:\\temp-{random.Next(100, 999)}.txt";

                byte[] fileData = wc.DownloadFile();

                using (FileStream fileStream = new(path, FileMode.Create))
                    fileStream.Write(fileData, 0, fileData.Length);*/
                using (var xClient = new DnsClient())
                {
                    xClient.Connect(new Address(192, 168, 1, 254)); //DNS Server address

                    /** Send DNS ask for a single domain name **/
                    xClient.SendAsk("github.com");

                    /** Receive DNS Response **/
                    Address destination = xClient.Receive(); //can set a timeout value

                    return term.DebugWrite($"File downloaded successfully at \"{destination}\".", 4);
                }
            }
            catch (Exception e)
            {
                return term.DebugWrite("Error downloading file: " + e.Message, 4);
            }
        }
    }

    public class Commands : Command
    {
        public Commands(String name, String description) : base(name, description) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
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

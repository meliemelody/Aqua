using Aqua.Commands;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using CosmosFtpServer;
using System;
using System.Text;
using term = Aqua.Terminal.Terminal;
using Cosmos.System.Network.IPv4.UDP.DNS;
using PrismNetwork;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Aqua.Network
{
    class HTTPClient
    {
        private static string request = string.Empty;
        private static TcpClient tcpc = new TcpClient(80);
        private static Address dns = new Address(8, 8, 8, 8);
        private static EndPoint endPoint = new EndPoint(dns, 80);
        public static bool ParseHeader()
        {
            return false;
        }
        public string Get(string url)
        {
            request +=
                "GET " + GetResource(url) + " HTTP/1.1\n" +
                "Host: " + GetHost(url) + "\n" +
                "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:88.0) Gecko/20100101 Firefox/88.0\n" +
                "";
            using (var xClient = new DnsClient())
            {
                xClient.Connect(dns); //DNS Server address

                /** Send DNS ask for a single domain name **/
                xClient.SendAsk(GetHost(url));

                /** Receive DNS Response **/
                Address destination = xClient.Receive(10000); //can set a timeout value

                xClient.Close();
                tcpc.Connect(destination, 80);
                tcpc.Send(Encoding.ASCII.GetBytes(request));
                endPoint.Address = destination;
                endPoint.Port = 80;
                return Encoding.ASCII.GetString(tcpc.Receive(ref endPoint));
            }

        }
        public static string GetHost(string url)
        {
            string newurl = url;
            if (newurl.StartsWith("http://"))
            {
                newurl = newurl.Remove(0, 7);
            }
            else if (newurl.StartsWith("https://"))
            {
                throw new Exception("HTTPS not supported!");
            }
            string[] spliturl = newurl.Split("/");
            return spliturl[0];
        }
        public static string GetResource(string url)
        {
            string newurl = url;
            if (newurl.StartsWith("http://"))
            {
                newurl = newurl.Remove(0, 7);
            }
            else if (newurl.StartsWith("https://"))
            {
                throw new Exception("HTTPS not supported!");
            }
            string[] spliturl = newurl.Split("/");
            for (int i = 1; i < spliturl.Length - 1; i++)
            {
                newurl += spliturl[i];
            }
            return newurl;
        }
    }

    public class Network
    {
        public Network() { }
        public static DHCPClient xClient = new DHCPClient();
        public static void Setup()
        {
            /** Send a DHCP Discover packet **/
            //This will automatically set the IP config after DHCP response
            xClient.SendDiscoverPacket();

            Address dnsAddress = new Address(8, 8, 8, 8);
            DNSConfig.Add(dnsAddress);

            Kernel.isNetworkConnected = true;
        }
    }

    public class PackageManager : Command
    {
        public PackageManager(string name, string description) : base(name, description) { }

        public override string Execute(string[] args)
        {
            try
            {
                WebClient wc = new WebClient(args[0]);
                Random random = new Random();
                string path = $"0:\\temp-{random.Next(100, 999)}.txt";

                byte[] fileData = wc.DownloadFile();

                using (FileStream fileStream = new(path, FileMode.Create))
                    fileStream.Write(fileData, 0, fileData.Length);

                return term.DebugWrite($"File downloaded successfully at \"{path}\".", 4);
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

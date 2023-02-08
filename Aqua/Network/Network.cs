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
    enum HTTPRequest
    {
        GET = 0,
        POST = 1,
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
        }

        public static string Get(string address, string port, string message)
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

        public static void DownloadFile(Address address, string filename)
        {
            try
            {
                var tcpClient = new TcpClient(80);

                tcpClient.Connect(address, 80);

                string httpget = "GET / HTTP/1.1\r\n" +
                                 "User-Agent: Wget (CosmosOS)\r\n" +
                                 "Accept: */*\r\n" +
                                 "Accept-Encoding: identity\r\n" +
                                 "Host: " + address + "\r\n" +
                                 "Connection: Keep-Alive\r\n\r\n";

                tcpClient.Send(Encoding.ASCII.GetBytes(httpget));

                var ep = new EndPoint(Address.Zero, 0);
                var data = tcpClient.Receive(ref ep);
                Console.WriteLine(data);

                tcpClient.Close();

                string httpresponse = Encoding.ASCII.GetString(data);

                Console.WriteLine(httpresponse);

                if (httpresponse != null)
                    File.WriteAllText(filename, httpresponse);
                else
                    Console.WriteLine("no");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
            }
        }
    }

    public class PackageManager : Command
    {
        public PackageManager(string name, string description) : base(name, description) { }

        public override string Execute(string[] args)
        {
            //return Install(args[0], args[1], args[2]);
            return Network.Get(args[0], args[1], args[2]);
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

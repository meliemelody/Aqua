using Cosmos.System.Network;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System;
using System.IO;

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
}

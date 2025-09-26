using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Fong.Helpers {
    public class NetworkHelper {
        static void GetNetworkAddresses() {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces()) {
                // Only consider active Ethernet or Wi-Fi interfaces
                if (ni.OperationalStatus != OperationalStatus.Up ||
                    (ni.NetworkInterfaceType != NetworkInterfaceType.Ethernet &&
                     ni.NetworkInterfaceType != NetworkInterfaceType.Wireless80211)) {
                    continue;
                }

                var ipProps = ni.GetIPProperties();

                foreach (var unicast in ipProps.UnicastAddresses) {
                    if (unicast.Address.AddressFamily != AddressFamily.InterNetwork) continue; // IPv4 only

                    var ipAddress = unicast.Address.ToString();
                    var subnetMask = unicast.IPv4Mask.ToString();

                    // Calculate network address
                    var networkAddress = GetNetworkAddress(unicast.Address, unicast.IPv4Mask);

                    // Convert subnet mask to CIDR
                    var cidr = SubnetMaskToCidr(unicast.IPv4Mask);

                    Console.WriteLine($"Interface: {ni.Name}");
                    Console.WriteLine($"IP Address: {ipAddress}");
                    Console.WriteLine($"Subnet Mask: {subnetMask}");
                    Console.WriteLine($"Network: {networkAddress}/{cidr}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }

        static string GetNetworkAddress(IPAddress ip, IPAddress mask) {
            var ipBytes = ip.GetAddressBytes();
            var maskBytes = mask.GetAddressBytes();

            if (ipBytes.Length != maskBytes.Length)
                throw new ArgumentException("IP address and subnet mask lengths do not match.");

            var result = new byte[ipBytes.Length];
            for (var i = 0; i < ipBytes.Length; i++) {
                result[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }

            return new IPAddress(result).ToString();
        }

        static int SubnetMaskToCidr(IPAddress mask) {
            var bytes = mask.GetAddressBytes();
            var cidr = 0;

            foreach (var b in bytes) {
                cidr += CountBits(b);
            }

            return cidr;
        }

        static int CountBits(byte b) {
            var count = 0;
            while (b != 0) {
                count += b & 1;
                b >>= 1;
            }

            return count;
        }
    }
}
using System.Net;

namespace NetworkUtility.Ping
{
    public class NetworkService
    {
        public static bool CheckIsLocalhost(string ipAddress)
        {
            return ipAddress == "127.0.0.1" || ipAddress == "localhost";
        }

        public static string GetLocalhost()
        {
            return "localhost";
        }

        public static bool IsValidIpAddress(string ipAddress)
        {
            if (ipAddress == "localhost")
            {
                ipAddress = "127.0.0.1";
            }
            return IPAddress.TryParse(ipAddress, out _);
        }
    }
}
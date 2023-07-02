using System.Net;

namespace Network
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // chú ý là prefix phải kết thúc là /
            var server = new MyHttpSever(new string []{"http://localhost:8080/"});
            await server.Start();
        }
    }
}
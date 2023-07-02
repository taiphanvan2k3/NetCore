using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;

namespace Networking
{
    public class Program
    {
        private static void Example1()
        {
            string url = "https://xuanthulab.net/lap-trinh/csharp/?page=3#acff";
            var uri = new Uri(url);

            // Dùng type để đọc tất cả properties của 1 uri
            var uritype = typeof(Uri);
            uritype.GetProperties().ToList().ForEach(property =>
            {
                Console.WriteLine($"{property.Name,15} {property.GetValue(uri)}");
            });
            Console.WriteLine($"Segments: {string.Join(",", uri.Segments)}");
        }

        private static void Example2()
        {
            // Lấy ra hostname của máy hiện tại
            var hostname = Dns.GetHostName();
            System.Console.WriteLine(hostname);
        }

        private static void Example3()
        {
            // string url = "https://xuanthulab.net";
            string url = "https://www.boostrapcdn.com/";
            var uri = new Uri(url);

            // Lấy ra Scheme: https
            System.Console.WriteLine(uri.Scheme);

            // Host: xuanthublab.net
            System.Console.WriteLine(uri.Host);

            IPHostEntry ipHostEntry = Dns.GetHostEntry(uri.Host);
            System.Console.WriteLine(ipHostEntry.HostName);

            // In ra địa chỉ IP của host
            // Một tên miền có thể trỏ đến nhiều server
            ipHostEntry.AddressList.ToList().ForEach(ip =>
            {
                System.Console.WriteLine(ip);
            });
        }

        private static void CheckServerIsAlive()
        {
            // Dùng Ping
            var ping = new Ping();

            // Truyền vào hostname
            var pingReply = ping.Send("facebook.com");

            System.Console.WriteLine(pingReply.Status);

            if (pingReply.Status == IPStatus.Success)
            {
                System.Console.WriteLine(pingReply.RoundtripTime);
                // Lấy địa chỉ phản hồi
                System.Console.WriteLine(pingReply.Address);
            }
        }

        private static void Example4()
        {
            // Do đây không phải phương thức bất đồng bộ nên ta mới dùng Wait() rồi result
            // Nếu đây là phương thức bất đồng bộ thì chỉ cần await HttpExample.GetWebContent("https://www.google.com?q=xuanthulab"); là được
            var task = HttpExample.GetWebContent("https://www.google.com?q=xuanthulab");
            task.Wait();

            var html = task.Result;
            System.Console.WriteLine(html);
        }

        private static void DownloadDataBytes()
        {
            var task = HttpExample.DownloadDataBytes("https://raw.githubusercontent.com/xuanthulabnet/jekyll-example/master/images/jekyll-01.png");
            task.Wait();

            var bytes = task.Result;
            string fileName = "1.png";

            // Ghi nội dung bytes ra file
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                // Dùng FileMode.CreateNew nếu tên file đã tồn tại và muốn tạo file mới
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        private static void DownloadStream()
        {
            var task = HttpExample.DownloadStream("https://raw.githubusercontent.com/xuanthulabnet/jekyll-example/master/images/jekyll-01.png", "download-stream.png");
            task.Wait();
        }

        static async Task Main(string[] args)
        {
            // CheckServerIsAlive();
            // DownloadDataBytes();
            // DownloadStream();
            // await HttpRequestExample.PostMethod2();
            await HttpRequestExample.MultiPartFormData();
        }
    }
}
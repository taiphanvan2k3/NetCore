using System.Net;
using System.Text;

namespace Network
{
    public class HttpListenerExample
    {
        public static async Task CheckHasSupportedAsync()
        {
            if (HttpListener.IsSupported)
            {
                System.Console.WriteLine("Supported");
            }
            else
            {
                System.Console.WriteLine("Not supported");
                throw new Exception("Not support HttpListener");
            }

            var server = new HttpListener();

            // Trước khi server cần cấu hình
            // Dòng cấu hình sau sẽ chấp nhận mọi host và lắng nghe ở cổng 8080
            // Để add kiểu: server.Prefixes.Add("http://*:8080/"); thì cần: netsh http add urlacl url=http://*:8080/ user=Administrator
            // Cổng port này là mình chọn cổng nào cũng được.
            server.Prefixes.Add("http://localhost:8080/");

            server.Start();
            System.Console.WriteLine("Server HTTP Start");

            do
            {
                // Ta tạo vòng lặp để server luôn lắng nghe request này đến request khác

                // Khi server start thì phải gọi ngay phương thức GetContextAsync() ngay
                // để chờ và tạo kết nối từ client đến server. Nếu không GetContext thì server
                // sẽ kết thúc ngay.
                HttpListenerContext httpListenerContext = await server.GetContextAsync();
                System.Console.WriteLine("Client connected");

                HttpListenerResponse response = httpListenerContext.Response;
                response.Headers.Add("content-type", "text/html");
                var outputStream = response.OutputStream;

                var html = "<h1>Hello world</h1>";
                var bytes = Encoding.UTF8.GetBytes(html);

                await outputStream.WriteAsync(bytes, 0, bytes.Length);
                // Sau khi đóng stream thì client sẽ nhận được nội dung này
                outputStream.Close();
            } while (server.IsListening);
        }


    }
}
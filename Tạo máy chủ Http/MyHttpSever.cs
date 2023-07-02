using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Network
{
    public class MyHttpSever
    {
        private HttpListener listener;

        public MyHttpSever(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                throw new Exception("HttpListener is not supported");
            }

            listener = new HttpListener();
            foreach (string prefix in prefixes)
            {
                listener.Prefixes.Add(prefix);
            }
        }

        public async Task Start()
        {
            listener.Start();
            System.Console.WriteLine("HTTP Server start");
            do
            {
                System.Console.WriteLine(DateTime.Now.ToLongTimeString() + " waiting a client connect");
                var context = await listener.GetContextAsync();
                System.Console.WriteLine("Client connected");

                await ProcessRequestAsync(context);
            } while (listener.IsListening);
        }

        public async Task ProcessRequestAsync(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            System.Console.WriteLine($"{request.HttpMethod} {request.RawUrl} {request.Url.AbsolutePath}");

            var outputStream = response.OutputStream;

            byte[] buffer = null;
            switch (request.Url.AbsolutePath)
            {
                case "/":
                    {
                        buffer = Encoding.UTF8.GetBytes("Xin chao cac ban");
                    }
                    break;
                case "/product":
                    {
                        response.Headers.Add("Content-type", "application/json");
                        var product = new
                        {
                            Name = "Thinkpad P15s nè",
                            Price = 31000
                        };

                        var json = JsonConvert.SerializeObject(product);
                        buffer = Encoding.UTF8.GetBytes(json);
                    }
                    break;

                case "/anh2.png":
                    {
                        response.Headers.Add("Content-type", "image/jpg");
                        buffer = await File.ReadAllBytesAsync("avatar.jpg");
                    }
                    break;
                default:
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        buffer = Encoding.UTF8.GetBytes("Not found");
                    }
                    break;
            }

            // Phải thiết lập trước khi respone được gửi đi
            response.ContentLength64 = buffer.Length;
            await outputStream.WriteAsync(buffer);

            //Cần phải đóng kết nối, chứ không là server để trả về cho client
            // Nội dung trả về cho client sẽ là nội dung có trong stream này
            outputStream.Close();
        }
    }
}
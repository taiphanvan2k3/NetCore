using System.Net;
using System.Net.Http.Headers;

namespace Networking2
{
    public class Program
    {
        private static async Task Example1()
        {
            string url = "https://postman-echo.com/post";
            var cookies = new CookieContainer();
            using var handler = new SocketsHttpHandler();

            // Cho phép chuyển tự động chuyển hướng sang URL mới
            // khi StatusCode trả về là 301
            handler.AllowAutoRedirect = true;
            handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            handler.UseCookies = true;

            // Sau khi được truy vấn nếu có các cookie thì các cookie
            // đó được lưu trong CookieContainer
            handler.CookieContainer = cookies;

            // Nếu không thiết lập tham số cho HttpClient thì nó sử dụng
            // HttpClientHandler làm mặc định
            using var httpClient = new HttpClient(handler);

            try
            {
                using var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.RequestUri = new Uri(url);
                httpRequestMessage.Headers.Add("User-agent", "Mozilla/5.0");

                var parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("Key1", "value1"));
                parameters.Add(new KeyValuePair<string, string>("Key2", "value2"));

                httpRequestMessage.Content = new FormUrlEncodedContent(parameters);

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                System.Console.WriteLine("List cookie");
                cookies.GetCookies(new Uri(url)).ToList().ForEach(c =>
                {
                    System.Console.WriteLine($"{c.Name} : {c.Value}");
                });
                System.Console.WriteLine();
                var html = await httpResponseMessage.Content.ReadAsStringAsync();
                // System.Console.WriteLine(html);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        private static async Task Example2()
        {
            // string url = "https://www.facebook.com/xuanthulab";
            string url = "https://www.google.com/taiphanvan2k3";
            CookieContainer cookies = new CookieContainer();

            // Tạo chuỗi handler
            var bottomHandler = new MyHttpClientHandler(cookies); // handler đáy (cuối)
            var changeUriHandler = new ChangeUri(bottomHandler);
            var denyAccessFacebook = new DenyAccessFacebook(changeUriHandler); // handler đỉnh

            // Khởi tạo HttpClient với hander đỉnh chuỗi hander
            var httpClient = new HttpClient(denyAccessFacebook);

            // Thực hiện truy vấn
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string htmltext = await response.Content.ReadAsStringAsync();

            Console.WriteLine(htmltext);
        }

        private static async Task Example3()
        {
            string url = "https://postman-echo.com/post";
            CookieContainer cookies = new CookieContainer();

            // Tạo chuỗi handler
            var bottomHandler = new MyHttpClientHandler(cookies); // handler đáy (cuối)
            var changeUriHandler = new ChangeUri(bottomHandler);
            var denyAccessFacebook = new DenyAccessFacebook(changeUriHandler); // handler đỉnh

            // Khởi tạo HttpClient với hander đỉnh chuỗi hander
            var httpClient = new HttpClient(denyAccessFacebook);

            try
            {
                using var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.RequestUri = new Uri(url);
                httpRequestMessage.Headers.Add("User-agent", "Mozilla/5.0");

                var parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("Key1", "value1"));
                parameters.Add(new KeyValuePair<string, string>("Key2", "value2"));

                httpRequestMessage.Content = new FormUrlEncodedContent(parameters);

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                System.Console.WriteLine("List cookie");
                cookies.GetCookies(new Uri(url)).ToList().ForEach(c =>
                {
                    System.Console.WriteLine($"{c.Name} : {c.Value}");
                });
                System.Console.WriteLine();
                var html = await httpResponseMessage.Content.ReadAsStringAsync();
                System.Console.WriteLine(html);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        static async Task Main(string[] args)
        {
            await Example2();
        }
    }
}
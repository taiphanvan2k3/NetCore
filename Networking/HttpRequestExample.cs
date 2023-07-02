using System.Net.Mime;
using System.Text;

namespace Networking
{
    // Lớp này tạo ra dùng để SendAsync chứ không GetAsync() nữa
    public class HttpRequestExample
    {
        public static async Task GetMethod()
        {
            // Giúp giải phóng tài nguyên liên quan đến luồng, chẳng hạn như kết nối mạng, một cách tự động và chính xác.
            using var httpClient = new HttpClient();

            // Tạo ra đối tượng HttpRequestMessage
            // Đối tượng này sẽ biểu diễn Http request (gồm 3 thành phần: dòng đầu, header, body)
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.RequestUri = new Uri("https://www.google.com");

            httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0");

            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            // Show Header:
            HttpExample.ShowHeaders(httpResponseMessage.Headers);

            string html = await httpResponseMessage.Content.ReadAsStringAsync();
            System.Console.WriteLine(html);
        }

        public static async Task PostMethod1()
        {
            using var httpClient = new HttpClient();

            // Tạo ra đối tượng HttpRequestMessage
            // Đối tượng này sẽ biểu diễn Http request (gồm 3 thành phần: dòng đầu, header, body)
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;

            // Gửi đến máy chủ postman-echo cho phép post thử dữ liệu lên
            httpRequestMessage.RequestUri = new Uri("https://postman-echo.com/post");

            httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0");

            // Gửi data đi (Form html, upload file)
            // Ví dụ submit form html
            var parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string,string>("bookstore","Nha sach Thieu Nhi"),
                new KeyValuePair<string, string>("Address","Quang Nam"),
                new KeyValuePair<string, string>("books","Conan"),
                new KeyValuePair<string, string>("books","Doraemon")
            };

            var content = new FormUrlEncodedContent(parameters);
            httpRequestMessage.Content = content;

            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            // Show Header:
            HttpExample.ShowHeaders(httpResponseMessage.Headers);

            string html = await httpResponseMessage.Content.ReadAsStringAsync();
            System.Console.WriteLine(html);
        }

        public static async Task PostMethod2()
        {
            // Giúp giải phóng tài nguyên liên quan đến luồng, chẳng hạn như kết nối mạng, một cách tự động và chính xác.
            using var httpClient = new HttpClient();

            // Tạo ra đối tượng HttpRequestMessage
            // Đối tượng này sẽ biểu diễn Http request (gồm 3 thành phần: dòng đầu, header, body)
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;

            // Gửi đến máy chủ postman-echo cho phép post thử dữ liệu lên
            httpRequestMessage.RequestUri = new Uri("https://postman-echo.com/post");

            httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0");

            // Gửi đi dữ liệu dạng json thông qua StringContent
            string json = @"
            {
                ""collections"": [
                    {
                        ""id"": ""dac5eac9-148d-a32e-b76b-3edee9da28f7"",
                        ""name"": ""Cloud API"",
                        ""owner"": ""631643"",
                        ""uid"": ""631643-dac5eac9-148d-a32e-b76b-3edee9da28f7""
                    },
                    {
                        ""id"": ""f2e66c2e-5297-e4a5-739e-20cbb90900e3"",
                        ""name"": ""Sample Collection"",
                        ""owner"": ""631643"",
                        ""uid"": ""631643-f2e66c2e-5297-e4a5-739e-20cbb90900e3""
                    },
                    {
                        ""id"": ""f695cab7-6878-eb55-7943-ad88e1ccfd65"",
                        ""name"": ""Postman Echo"",
                        ""owner"": ""631643"",
                        ""uid"": ""631643-f695cab7-6878-eb55-7943-ad88e1ccfd65""
                    }
                ]
            }";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpRequestMessage.Content = content;

            // Respone
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            // Show Header:
            HttpExample.ShowHeaders(httpResponseMessage.Headers);

            string html = await httpResponseMessage.Content.ReadAsStringAsync();
            System.Console.WriteLine(html);
        }

        public static async Task MultiPartFormData()
        {
            // Giúp giải phóng tài nguyên liên quan đến luồng, chẳng hạn như kết nối mạng, một cách tự động và chính xác.
            using var httpClient = new HttpClient();

            // Tạo ra đối tượng HttpRequestMessage
            // Đối tượng này sẽ biểu diễn Http request (gồm 3 thành phần: dòng đầu, header, body)
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;

            // Gửi đến máy chủ postman-echo cho phép post thử dữ liệu lên
            httpRequestMessage.RequestUri = new Uri("https://postman-echo.com/post");

            httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0");

            var content = new MultipartFormDataContent();

            // Upload file nên ta cần tạo ra Stream để đọc file
            Stream fileStream = File.OpenRead("test.txt");
            var fileUpLoad = new StreamContent(fileStream);

            // Thêm StreamContent đó vào content
            content.Add(fileUpLoad, "fileUpload");

            // Thêm tiếp nội dung dưới dạng json
            string json = @"
            {""collections"": 
                [
                    {
                        ""id"": ""dac5eac9-148d-a32e-b76b-3edee9da28f7"",
                        ""name"": ""Cloud API"",""owner"": ""631643"",
                        ""uid"": ""631643-dac5eac9-148d-a32e-b76b-3edee9da28f7""
                    }
                ]
            }";

            json = json.Replace("\r\n", "");
            // Chỉ có MultipartFormDataContent mới par
            content.Add(new StringContent(json, Encoding.UTF8, "application/json"), "string-content");

            httpRequestMessage.Content = content;
            // Respone
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            // Show Header:
            HttpExample.ShowHeaders(httpResponseMessage.Headers);

            string html = await httpResponseMessage.Content.ReadAsStringAsync();
            byte[] bytes = Encoding.UTF8.GetBytes(html);
            Stream writeStream = new FileStream("output.txt", FileMode.Create, FileAccess.Write, FileShare.None);

            await writeStream.WriteAsync(bytes, 0, bytes.Length);
            await writeStream.FlushAsync();
            System.Console.WriteLine(html);
        }
    }
}
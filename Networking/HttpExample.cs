using System.Net.Http.Headers;

namespace Networking
{
    public class HttpExample
    {
        public static void ShowHeaders(HttpResponseHeaders headers)
        {
            System.Console.WriteLine("List of headers:");
            foreach (var header in headers)
            {
                // Type của header": KeyValuePair<string, IEnumerable<string>>
                System.Console.WriteLine(header.Key + " " + header.Value);
            }
            System.Console.WriteLine("======================================");
        }

        public static async Task<string> GetWebContent(string url)
        {
            // Tự động hủy đối tượng này khi thoát ra khỏi phương thức
            // nên ta dùng using
            using HttpClient httpClient = new HttpClient();

            try
            {
                // Thêm các request header dưới dạng <Key,Value>
                // Thêm bao nhiêu header cũng được nhưng phụ thuộc vào máy chủ có tiếp nhận không
                httpClient.DefaultRequestHeaders.Add("Accept", "text/html");

                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

                ShowHeaders(httpResponseMessage.Headers);

                // Lấy về nội dung respone body
                string html = await httpResponseMessage.Content.ReadAsStringAsync();
                return html;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static async Task<byte[]> DownloadDataBytes(string url)
        {
            // Tự động hủy đối tượng này khi thoát ra khỏi phương thức
            // nên ta dùng using
            using HttpClient httpClient = new HttpClient();

            try
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

                ShowHeaders(httpResponseMessage.Headers);

                // Lấy về nội dung respone body
                var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                return bytes;
            }
            catch
            {
                System.Console.WriteLine("Fail");
                return null;
            }
        }

        public static async Task DownloadStream(string url, string fileName)
        {
            using HttpClient httpClient = new HttpClient();
            try
            {
                var httpResponseMessage = await httpClient.GetAsync(url);

                using var stream = await httpResponseMessage.Content.ReadAsStreamAsync();

                // Có thể đọc từng byte/ khối byte từ stream này
                int SIZEBUFFER = 500;

                // Tạo ra vùng đệm gồm 500 bytes
                var buffer = new byte[SIZEBUFFER];

                // Liên quan đến việc ghi/đọc file cũng dùng using
                using var streamWrite = File.OpenWrite(fileName);
                bool eof = false;
                do
                {
                    int numBytes = await stream.ReadAsync(buffer, 0, SIZEBUFFER);
                    if (numBytes == 0)
                    {
                        eof = true;
                    }
                    else
                    {
                        await streamWrite.WriteAsync(buffer, 0, numBytes);
                    }
                } while (!eof);
            }
            catch
            {
                System.Console.WriteLine("Failed");
            }
        }
    }
}
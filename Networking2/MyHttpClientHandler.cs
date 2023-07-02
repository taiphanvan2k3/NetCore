using System.Net;

namespace Networking2
{
    public class MyHttpClientHandler : HttpClientHandler
    {
        public MyHttpClientHandler(CookieContainer cookieContainer)
        {
            CookieContainer = cookieContainer;
            AllowAutoRedirect = false;
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            UseCookies = true; // default: true sẵn rồi
        }

        // Bắt buộc phải override
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            System.Console.WriteLine("Starting: " + request.RequestUri.ToString());

            // Thực hiện request đến server
            var respone = await base.SendAsync(request,cancellationToken);
            System.Console.WriteLine("Done!!!");
            return respone;
        }
    }
}
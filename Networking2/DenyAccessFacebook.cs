using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Networking2
{
    // Lớp này ngăn chặn việc truy cập facebook
    public class DenyAccessFacebook : DelegatingHandler
    {
        // Tương tự với ChangeUri thì class này cũng phải khởi tạo handler
        public DenyAccessFacebook(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var host = request.RequestUri.Host.ToLower();
            Console.WriteLine($"Check in at DenyAccessFacebook - {host}");
            if (host.Contains("facebook.com"))
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(Encoding.UTF8.GetBytes("Không được truy cập"));
                // return await Task.FromResult<HttpResponseMessage>(response);
                return response;
            }
            // Chuyển truy vấn cho base (thi hành InnerHandler)
            return await base.SendAsync(request,cancellationToken);
        }
    }
}
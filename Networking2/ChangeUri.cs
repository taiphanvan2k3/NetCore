namespace Networking2
{
    // Kế thừa từ DelegateHandler để thực hiện pipeline
    public class ChangeUri : DelegatingHandler
    {
        // Khi Delegating khởi tạo thì bắt buộc phải khải tạo handler phía trước nó
        public ChangeUri(HttpMessageHandler innerHandler) : base(innerHandler) { }

        // Cũng phải override SendAsync
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Lấy ra host của url và kiểm tra có phải là google.com không
            var host = request.RequestUri.Host.ToLower();
            Console.WriteLine($"Check in at ChangeUri - {host}");
            if (host.Contains("google.com"))
            {
                // Đổi địa chỉ truy cập từ google.com sang github
                request.RequestUri = new Uri("https://github.com/");
            }

            // Chuyển truy vấn cho base (thi hành innerHandler)
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
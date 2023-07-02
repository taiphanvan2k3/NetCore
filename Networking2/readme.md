# Học về HttpMessageHandler
- Lớp HttpMessageHandler là lớp trừu tượng, nó là lớp cơ sở được thư viện .NET Core triển khai ra các lớp như DelegatingHandler, HttpMessageHandler, HttpClientHandler ... các lớp triển khai này (hoặc nếu tự xây dựng lớp triển khai HttpMessageHandler) thì phải nạp chồng phương thức SendAsync:

    ```protected Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, CancellationToken cancellationToken);```
- Các lớp triển khai HttpMessageHandler dùng để khởi tạo HttpClient, lúc này HttpCliet thực hiện gửi truy vấn (SendAsync) thì SendAsync của handler sẽ thực thi.
- CookieHandler sẽ chứa các Cookie mà HttpRespone trả về
- Nên sử dụng SocketsHttpHandler thay vì HttpClientHandler vì nó được xây dựng sau và hỗ trợ đa nền tảng hơn. 2 lớp này sử dụng giống hệt nhau nhưng SocketsHttpHandler được thiết kế để sử dụng tốt hơn - nhanh hơn trên .NET Core, nó độc lập thiết bị tốt hơn (chạy tốt trên macOS, Linux). 
- Chú ý, từ .NET Core 2.1 khuyến khích sử dụng SocketsHttpHandler thay cho HttpClientHandler

# HttpClientHandler hoặc SocketsHttpHandler
- Hai lớp này triển khai từ HttpMessageHandler, nó thực hiện cuối cùng trong chuỗi các handler nếu có để thực sự gửi truy vấn HTTP
- Mặc định HttpClient sẽ sử dụng handler là HttpClientHandler. Ta có thể thay đổi bằng cách sử dụng tham số là 1 đối tượng thuộc lớp SocketsHttpHandler.
- Một số thuộc tính trong HttpClientHandler hoặc SocketsHttpHandler
+ AllowAutoRedirect: Thuộc tính, mặc định là true, để thiết lập tự động chuyển hướng. Ví dụ truy vấn đến URI có chuyển hướng đến đích mới (301) thì - HttpClient sẽ tự động chuyển hướng truy vấn đến đó.
+ AutomaticDecompression: 
Thuộc tính thuộc tính để handler tự động giải nén / nén nội dung HTTP, nó thuộc kiểu enum DecompressionMethods gồm có:
    * DecompressionMethods.None không sử dụng nén
    * DecompressionMethods.GZip dùng thuật toán gZip
    * DecompressionMethods.Deflate dùng thuật toán nén deflate
Ví dụ có thể gán:
AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMeth
+ UseCookies: mặc định là true, cho phép sử dụng thuộc tính CookieContainer để lưu các Cookie của server khi respone trả về, cũng như tự động gửi Cookie khi gửi truy vấn
+ CookieContainer: thuộc tính thuộc lớp CookieContainer, nó lưu các cookie.

# DelegatingHandler
- Cũng triển khai từ HttpMessageHandler là một handler đặc biệt, nó như một MiddleWare để tạo ra một pipeline (chuỗi các handler). Mỗi đối tượng DelegatingHandler có một thuộc tính InnerHandler (kiểu HttpMessageHandler), phải được gán bằng một đối tượng SocketsHttpHandler, HttpClientHandler hoặc DelegatingHandler... Thiết lập InnerHandler qua phương thức khởi tạo lớp DelegatingHandler. Khi thực hiện truy vấn SendAsync thì nó tiếp tục gọi SendAsync trong InnerHandler, cứ như vậy nó sẽ tạo thành chuỗi.
- Xem ví dụ về cách chuyển khai DenyAccessFacebook.cs và ChangeUri.cs
- Nếu InnerHandler không phải là một DelegatingHandler thì InnerHandler đó phải là handler dưới cùng của chuỗi handler. 
- Request - respone sẽ đi qua chuỗi handler từ trên cùng xuống dưới khi truy vấn và ngược lại khi trả về. 
- Hình minh họa: 
[image](./image.png)
Sau khi có 3 loại Hander này thì tạo chúng thành chuỗi theo thức tự:
```
 * Request  --> ......................  -->  .............  -->  .......................
                . DenyAccessFacebook .       . ChangeUri .       . MyHttpClientHandler .
 * Response <-- ......................  <--  .............  <--  .......................
```
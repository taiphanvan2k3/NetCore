1. ItemsController yêu cầu user đã được Authorized mới có thể truy cập được
+ Một controller hay action được chỉ định Authorized thì cần configure Authorized tại program.cs. Nếu không thì sẽ bị lỗi:
`No authenticationScheme was specified, and there was no DefaultChallengeScheme found. The default schemes can be set
using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).`

2. Khi hệ thống nhận được một token, nó sẽ sử dụng IssuerSigningKey trong configure JwtBearer để kiểm tra xem token đó có được ký (chữ ký) bởi SigningCredentials cùng một key khi tạo token hay không. Nếu khóa không phù hợp, quá trình xác thực sẽ thất bại và token sẽ bị coi là không hợp lệ.

3. Role base authorization
    ```csharp
    [Authorize(Roles = "Admin")]  
    [Authorize(Roles = "Manager")]
    ```

    Khi viết cách này thì yêu cầu user phải có đủ 2 quyền thì mới có thể truy cập,
    còn khi viết [Authorize(Roles = "Admin,Manager")] thì yêu cầu thoả mãn 1 trong 2 quyền là có thể truy cập rồi

4. Cookie HttpOnly: do client không thể lấy giá trị của HttpOnly cookie nên nếu muốn lưu token dưới dạng HttpOnly Cookie thì tại server ta cần có một bước `đón nhận` request để từ đó trích xuất ra token trong cookie, từ đó dùng cho giai đoạn tiếp theo của JWT Authentication
    ```csharp
    // Một đoạn chương trình đón nhận request bên trong AddJwtBearer
    options.Events = new JwtBearerEvents()
    {
        OnMessageReceived = context =>
        {
            // Khi một request gửi tới, nó sẽ đọc giá trị của cookie "token" và sử dụng nó cho phần JWT Authentication. Lúc này đã có được token, tiếp theo sẽ validate nó theo các tiêu chí đặt ra của JWT đã chỉ ra trước đó
            context.Token = context.Request.Cookies["token"];
            return Task.CompletedTask;
        }
    };
    ```

5. Trong 1 controller API, một action cần chỉ định rõ nó là HttpGet hay Post cụ thể. Nếu không muốn xem nó là 1 endpoints thì có thể xử lý sau:
    ```cs
    [NonAction]
    // Hoặc chuyển về private hoặc dùng [NonAction]
    public dynamic JWTGenerator(UserDto user)
    ```
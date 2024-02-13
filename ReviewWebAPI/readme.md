- ItemsController yêu cầu user đã được Authorized mới có thể truy cập được
+ Một controller hay action được chỉ định Authorized thì cần configure Authorized tại program.cs. Nếu không thì sẽ bị lỗi:
`No authenticationScheme was specified, and there was no DefaultChallengeScheme found. The default schemes can be set
using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).`

- Khi hệ thống nhận được một token, nó sẽ sử dụng IssuerSigningKey trong configure JwtBearer để kiểm tra xem token đó
có được ký (chữ ký) bởi SigningCredentials cùng một key khi tạo token hay không. Nếu khóa không phù hợp, quá trình xác thực sẽ thất bại và token sẽ bị coi là không hợp lệ.

- `[Authorize(Roles = "Admin")]  
[Authorize(Roles = "Manager")]
`

Khi viết cách này thì yêu cầu user phải có đủ 2 quyền thì mới có thể truy cập,
còn khi viết [Authorize(Roles = "Admin,Manager")] thì yêu cầu thoả mãn 1 trong 2 quyền là có thể truy cập rồi

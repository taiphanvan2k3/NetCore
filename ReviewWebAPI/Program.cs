using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NetCore.ReviewWebAPI;
using ReviewWebAPI.Authorization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSetting>(
    builder.Configuration.GetSection("ApplicationSettings")
);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ApplicationSettings:Secret"])),

        // Yêu cầu token phải có claim iss, aud và chính xác iss, aud đã chỉ định thì mới coi như token đó hợp lệ
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = configuration["ApplicationSettings:Issuer"],
        ValidAudience = configuration["ApplicationSettings:Audience"],
    };
});

builder.Services.AddAuthorization(options =>
{
    // ========== Simple Policies ==========
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

    // Có ít nhất 1 role thoả mãn là được và giá trị của claim username phải thoả mãn ít nhất 2 giá trị đưa ra đó thì PASS
    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("Admin", "SuperAdmin").RequireClaim("username", "TaiPV", "taiphanvan2403"));


    // ========== Function Policies ==========
    // RequireAssertion dùng để định nghĩa một biểu thức Lamda, token phải chứa claim thoả mãn chính sách đưa ra 
    // thì mới có quyền truy cập được API đó.
    options.AddPolicy("ExclusiveContentPolicy", policy =>
        policy.RequireAssertion(context => context.User.HasClaim(claim => claim.Type == "username" && (claim.Value == "TaiPV" || claim.Value == "taiphanvan2403")
            || context.User.IsInRole("SuperAdmin"))));

    // Custom Policy, có thể thêm nhiều requirement vào AddRequirements
    options.AddPolicy("IsOldEnoughWithRole", policy => policy.AddRequirements(new IsOldEnoughWithRoleRequirement(18)));
    options.AddPolicy("IsStudentDUT", policy => policy.AddRequirements(new IsStudentDUTRequirement("DUT")));
});

builder.Services.AddSingleton<IAuthorizationHandler, IsOldEnoughWithRoleHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, IsStudentDUTHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

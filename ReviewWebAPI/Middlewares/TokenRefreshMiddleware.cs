using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCore.ReviewWebAPI;
using NetCore.ReviewWebAPI.Services;
using ReviewWebAPI.Models.Schemas;

namespace ReviewWebAPI.Middlewares
{
    public class TokenRefreshMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenRefreshMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITokenService tokenService, IOptions<AppSetting> appSetting)
        {
            var accessToken = context.Request.Cookies["X-Access-Token"];
            var refreshToken = context.Request.Cookies["X-Refresh-Token"];

            if (string.IsNullOrEmpty(accessToken) || !IsTokenValid(accessToken, appSetting.Value.Secret))
            {
                // Access token không hợp lệ hoặc đã hết hạn
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var user = ListUsers.fakeUsersList.FirstOrDefault(x => x.Token == refreshToken && x.TokenExpires >= DateTime.Now);
                    if (user != null)
                    {
                        // Refresh token và cập nhật access token mới
                        var newToken = tokenService.JWTGenerator(user, context).accessToken;
                        context.Request.Headers["Authorization"] = "Bearer " + newToken;
                    }
                }
            }


            await _next(context);
        }

        private static bool IsTokenValid(string token, string secret)
        {
            try
            {
                // Sử dụng thư viện JWT để giải mã token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? "")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // Thiết lập không chấp nhận sự chệch thời gian
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                // Kiểm tra tính hợp lệ của token
                return principal.Identity.IsAuthenticated;
            }
            catch (Exception)
            {
                // Token không hợp lệ
                return false;
            }
        }
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReviewWebAPI.Models.Schemas;

namespace NetCore.ReviewWebAPI.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// Sinh ra một access token và thay đổi lại refresh token luôn<br/> 
        /// Created: 2024/03/09
        /// </summary>
        /// <returns></returns>
        public dynamic JWTGenerator(UserDto user, HttpContext context);
    }

    public class TokenService : ITokenService
    {
        private readonly AppSetting _appSetting;
        public TokenService(IOptions<AppSetting> appSetting)
        {
            _appSetting = appSetting?.Value ?? throw new ArgumentNullException(nameof(appSetting));
        }

        public dynamic JWTGenerator(UserDto user, HttpContext context)
        {
            var encryptToken = GenerateAccessToken(user);
            SetJWT(context, encryptToken);

            RefreshToken refreshToken = GenerateRefreshToken();
            SetRefreshToken(context, refreshToken, user);

            return new
            {
                accessToken = encryptToken,
                refreshToken,
                username = user.UserName
            };
        }

        private string GenerateAccessToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Ở đây dùng mã ASCII cũng được vì secret key thường là tiếng Anh nên bảng mã ASCII có thể biểu diễn được
            var key = Encoding.ASCII.GetBytes(_appSetting.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("username", user.UserName),
                    new Claim("iss", _appSetting.Issuer),
                    new Claim("aud", _appSetting.Audience),

                    // Lúc này trong token sẽ có 2 role là HR và giá trị của user.Role
                    //new Claim(ClaimTypes.Role, "Employee"),
                    new Claim(ClaimTypes.Role, user.Role),

                    new Claim(ClaimTypes.DateOfBirth, user.BirthDay.ToShortDateString()),
                    new Claim("school", user.SchoolName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),

                // key này phải giống với key lúc configure JWT Bearer
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptToken = tokenHandler.WriteToken(token);
            return encryptToken;
        }

        private static RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                // Token này không cần phải là JWT mà nó chỉ cần là 1 string bất kì là được rồi
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private static void SetJWT(HttpContext context, string encryptToken)
        {
            context.Response.Cookies.Append("X-Access-Token", encryptToken, new CookieOptions
            {
                // Access token có thời hạn ngắn
                Expires = DateTime.UtcNow.AddMinutes(15),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });
        }

        private static void SetRefreshToken(HttpContext context, RefreshToken refreshToken, UserDto user)
        {
            context.Response.Cookies.Append("X-Refresh-Token", refreshToken.Token, new CookieOptions()
            {
                Expires = refreshToken.Expires,
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });

            UserDto userInDB = ListUsers.fakeUsersList.Where(u => u.UserName == user.UserName).FirstOrDefault()
                ?? throw new Exception("This user is not exist");
            userInDB.Token = refreshToken.Token;
            userInDB.TokenCreated = refreshToken.Created;
            userInDB.TokenExpires = refreshToken.Expires;
        }
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCore.ReviewWebAPI;
using ReviewWebAPI.Models.Schemas;

namespace ReviewWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static readonly List<UserDto> fakeUsersList = new();
        private readonly AppSetting _appSetting;

        public AuthController(IOptions<AppSetting> appSetting)
        {
            _appSetting = appSetting?.Value ?? throw new ArgumentNullException(nameof(appSetting));
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            var user = fakeUsersList
                .FirstOrDefault(u => u.UserName == model.UserName);
            if (user == null)
            {
                return BadRequest("Username is not exist");
            }

            var match = CheckPassword(model.Password, user);
            if (!match)
            {
                return BadRequest("Password is incorrect");
            }

            // Tạo token
            var tokenHandler = new JwtSecurityTokenHandler();

            // Ở đây dùng mã ASCII cũng được vì secret key thường là tiếng Anh nên bảng mã ASCII có thể biểu diễn được
            var key = Encoding.ASCII.GetBytes(_appSetting.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("username", user.UserName),
                    new Claim("iss", _appSetting.Issuer),
                    new Claim("aud", _appSetting.Audience),
                    new Claim(ClaimTypes.Role, "Client")
                }),
                Expires = DateTime.UtcNow.AddDays(7),

                // key này phải giống với key lúc configure JWT Bearer
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Này là 1 string
            var encrypterToken = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = encrypterToken,
                userName = user.UserName
            });
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDto model)
        {
            UserDto user = new() { UserName = model.UserName };
            if (model.ConfirmPassword == model.Password)
            {
                using HMACSHA512 hMACSHA512 = new();

                // Cần lưu lại key cho lúc validate đăng nhập
                user.PasswordSalt = hMACSHA512.Key;
                user.PasswordHash = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
            }
            else
            {
                return BadRequest("Confirm password doesn't match");
            }

            fakeUsersList.Add(user);
            return Ok(user);
        }

        private static bool CheckPassword(string password, UserDto user)
        {
            using HMACSHA512 hMACSHA512 = new(user.PasswordSalt);
            var compute = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(password));
            return compute.SequenceEqual(user.PasswordHash);
        }
    }
}
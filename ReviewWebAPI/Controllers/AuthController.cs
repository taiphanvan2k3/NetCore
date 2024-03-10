using Microsoft.AspNetCore.Mvc;
using NetCore.ReviewWebAPI.Services;
using ReviewWebAPI.Models.Schemas;
using System.Security.Cryptography;
using System.Text;

namespace ReviewWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            var user = ListUsers.fakeUsersList
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

            var tokens = _tokenService.JWTGenerator(user, HttpContext);
            return Ok(new
            {
                tokens.accessToken,
                tokens.refreshToken.Token,
                tokens.username
            });
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDto model)
        {
            UserDto user = new()
            {
                UserName = model.UserName,
                Role = model.Role,
                BirthDay = model.BirthDay,
                SchoolName = model.SchoolName
            };
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

            ListUsers.fakeUsersList.Add(user);
            return Ok(user);
        }

        private static bool CheckPassword(string password, UserDto user)
        {
            using HMACSHA512 hMACSHA512 = new(user.PasswordSalt);
            var compute = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(password));
            return compute.SequenceEqual(user.PasswordHash);
        }

        /// <summary>
        /// Gọi API này để refresh lại access token
        /// </summary>
        /// <returns></returns>
        [HttpGet("refresh-token")]
        public ActionResult<string> RefreshToken()
        {
            var refreshToken = Request.Cookies["X-Refresh-Token"];
            var user = ListUsers.fakeUsersList.Where(x => x.Token == refreshToken).FirstOrDefault();

            if (user == null || user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token has expired");
            }
            _tokenService.JWTGenerator(user, HttpContext);
            return Ok();
        }
    }
}
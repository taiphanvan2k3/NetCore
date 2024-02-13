using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Namespace
{
    [Route("api/[controller]")]
    [ApiController]

    // Chỉ thoả mãn Policy AdminOnly mới có thể truy cập controller này
    // [Authorize(Policy = "AdminOnly")]

    // Sử dụng function Policy
    // [Authorize(Policy = "ExclusiveContentPolicy")]

    // Sử dụng Custom Policy
    [Authorize(Policy = "IsOldEnoughWithRole")]
    public class StudentController : ControllerBase
    {
        public List<string> studentList = new() { "Tài", "Tiến", "Hiệu", "T.Anh" };

        [HttpGet]
        public List<string> GetStudentList()
        {
            try
            {
                return studentList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("school-infor")]

        // Sẽ cần validate 2 lần policy IsOldEnoughWithRole và IsStudentDUT.
        // Nó sẽ vào các hàm xử lí của mỗi Handler để validate trước khi cho phép gọi hàm GetSchoolInfor
        [Authorize(Policy = "IsStudentDUT")]
        public IActionResult GetSchoolInfor()
        {
            string username = HttpContext.User.FindFirst("username").Value;
            return Ok(new
            {
                welcome = $"Hello everyone, I am {username} and I am an student of DUT."
            });
        }

        [HttpGet("super-admin")]

        // Phải thoả mãn Policy của Controller trước đã rồi mới xuống được đây check Policy của Action
        [Authorize(Policy = "SuperAdminOnly")]
        public IActionResult GetSuperAdminInfor()
        {
            string username = HttpContext.User.FindFirst("username").Value;
            string roles = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            return Ok(new
            {
                username,
                role = roles
            });
        }
    }
}
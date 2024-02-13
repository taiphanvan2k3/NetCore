using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReviewWebAPI.Controllers
{
    [Route("api/[controller]")]

    // Khi muốn có cả 2 role mới có thể truy cập
    //[Authorize(Roles = "Admin")]
    //[Authorize(Roles = "Manager")]

    [Authorize(Roles = "Admin, Employee, HR")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public List<string> colorList = new() { "Blue", "Red", "Green", "Yellow", "Pink" };

        [HttpGet]
        public List<string> GetColorList()
        {
            try
            {
                var context = HttpContext.User;
                return colorList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
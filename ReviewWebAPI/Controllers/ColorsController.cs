using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReviewWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReviewWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
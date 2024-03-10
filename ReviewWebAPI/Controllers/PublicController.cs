using Microsoft.AspNetCore.Mvc;

namespace ReviewWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        [HttpGet]
        public IActionResult CheckStatus()
        {
            try
            {
                return Ok(new
                {
                    State = HttpContext.User.Identity.IsAuthenticated
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("do-something")]
        public IActionResult DoSomeThing()
        {
            return Ok(new
            {
                State = HttpContext.User.Identity.IsAuthenticated,
                Message = "Oke"
            });
        }
    }
}
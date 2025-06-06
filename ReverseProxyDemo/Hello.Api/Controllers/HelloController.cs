using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("hello-world")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("Hello from Backend API!");
}

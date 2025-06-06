using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("hello-world")]
public class HelloController : ControllerBase
{
    /// <summary>
    /// Handles HTTP GET requests and returns a greeting message.
    /// </summary>
    /// <returns>An HTTP 200 OK response with the message "Hello from Backend API!".</returns>
    [HttpGet]
    public IActionResult Get() => Ok("Hello from Backend API!");
}

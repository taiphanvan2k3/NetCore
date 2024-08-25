using Microsoft.AspNetCore.Mvc;

namespace SerilogWithSeq.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(ILogger<UsersController> logger) : ControllerBase
    {
        private readonly ILogger<UsersController> _logger = logger;

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logger.LogInformation("Getting all users");
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            _logger.LogInformation($"Getting user with ID {id}");
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
            _logger.LogInformation($"Creating new user with value {value}");
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            _logger.LogInformation($"Updating user with ID {id} and value {value}");
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogInformation($"Deleting user with ID {id}");
        }
    }
}

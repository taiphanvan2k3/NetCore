using LearnDapper.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearnDapper.Controllers
{
    [Route("api/teams")]
    [ApiController]
    public class TeamController(ITeamRepository teamRepository) : ControllerBase
    {
        private readonly ITeamRepository _teamRepository = teamRepository
            ?? throw new ArgumentNullException(nameof(teamRepository));
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var teams = await _teamRepository.GetListOfTeams();
            return Ok(teams);
        }
    }
}
using LearnDapper.Interfaces;
using LearnDapper.Repositories.PlayerRepo.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace LearnDapper.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController(IPlayerRepository playerRepository) : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository = playerRepository
            ?? throw new ArgumentNullException(nameof(playerRepository));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayers(int id)
        {
            var players = await _playerRepository.GetPlayers();
            return Ok(players);
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            var players = await _playerRepository.GetPlayers();
            return Ok(players);
        }

        [HttpGet("with-team")]
        public async Task<IActionResult> GetPlayersWithTeam()
        {
            var players = await _playerRepository.GetPlayersWithTeam();
            return Ok(players);
        }

        [HttpGet("{id}/profile")]
        public async Task<IActionResult> GetPlayerProfile(int id)
        {
            var player = await _playerRepository.GetPlayerProfile(id);
            return Ok(player);
        }

        [HttpGet("{id}/profile-multiple-level")]
        public async Task<IActionResult> GetPlayerProfileMultipleLevel(int id)
        {
            var player = await _playerRepository.GetPlayerDetailWithMultipleLevel(id);
            return Ok(player);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayer(PlayerCreateUpdateDto player)
        {
            var playerId = await _playerRepository.CreatePlayer(player);
            return CreatedAtAction(nameof(GetPlayerProfile), new { id = playerId }, playerId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, PlayerCreateUpdateDto player)
        {
            await _playerRepository.UpdatePlayer(id, player);
            return NoContent();
        }

        [HttpPut("multiple")]
        public async Task<IActionResult> UpdateMultiplePlayers()
        {
            await _playerRepository.UpdateMultiplePlayers();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            await _playerRepository.DeletePlayer(id);
            return NoContent();
        }
    }
}
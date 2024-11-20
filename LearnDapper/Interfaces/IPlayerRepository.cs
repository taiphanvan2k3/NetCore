using System.Diagnostics.Eventing.Reader;
using LearnDapper.Databases.Schemas;
using LearnDapper.Repositories.PlayerRepo.Schemas;

namespace LearnDapper.Interfaces
{
    public interface IPlayerRepository
    {
        public Task<List<Player>> GetPlayers();
        public Task<List<Player>> GetPlayersWithTeam();
        public Task<Player> GetPlayer(int id);
        public Task<PlayerDetail> GetPlayerProfile(int id);
        public Task<PlayerDetailDto> GetPlayerDetailWithMultipleLevel(int id);
        public Task<bool> CreatePlayer(PlayerCreateUpdateDto player);
        public Task<bool> UpdatePlayer(int id, PlayerCreateUpdateDto player);
        public Task<bool> UpdateMultiplePlayers();
        public Task<bool> DeletePlayer(int id);
    }
}
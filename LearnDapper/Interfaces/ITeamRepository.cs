using LearnDapper.Databases.Schemas;
using LearnDapper.Repositories.TeamRepo.Schemas;

namespace LearnDapper.Interfaces
{
    public interface ITeamRepository
    {
        public Task<List<TeamDetail>> GetListOfTeams();
    }
}
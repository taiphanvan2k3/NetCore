using Dapper;
using LearnDapper.Databases;
using LearnDapper.Databases.Schemas;
using LearnDapper.Interfaces;
using LearnDapper.Repositories.TeamRepo.Schemas;

namespace LearnDapper.Repositories.TeamRepo
{
    public class TeamRepository(DapperContext context) : ITeamRepository
    {
        private readonly DapperContext _context = context
            ?? throw new ArgumentNullException(nameof(context));

        public async Task<List<TeamDetail>> GetListOfTeams()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT t.Id as TeamId, t.CountryName as TeamName, p.Id as PlayerId, p.Name as PlayerName" +
                " from Teams t join Players p on t.Id = p.TeamId";

            var teams = await connection.QueryAsync<TeamDetail, PlayerDto, TeamDetail>(sql,
                (team, player) =>
                {
                    team.Players.Add(player);
                    return team;
                },
                splitOn: "PlayerId"
            );

            var groupedTeams = teams.GroupBy(t => t.TeamId).Select(g =>
            {
                var team = g.First();
                team.Players = g.Select(t => t.Players.Single()).ToList();
                return team;
            })
            .ToList();

            return groupedTeams;
        }
    }
}
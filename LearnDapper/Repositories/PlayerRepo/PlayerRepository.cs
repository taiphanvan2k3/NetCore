using System.Data;
using Dapper;
using LearnDapper.Databases;
using LearnDapper.Databases.Schemas;
using LearnDapper.Interfaces;
using LearnDapper.Repositories.PlayerRepo.Schemas;

namespace LearnDapper.Repositories.PlayerRepo
{
    public class PlayerRepository(DapperContext context) : IPlayerRepository
    {
        private readonly DapperContext _context = context
            ?? throw new ArgumentNullException(nameof(context));

        /// <summary>
        /// Sử dụng Procedure nhưng chỉ lấy về object có 1 cấp độ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlayerDetail> GetPlayerProfile(int id)
        {
            using var connection = _context.CreateConnection();
            var procedure = "GetPlayerProfile";
            var parameters = new DynamicParameters(new { PlayerId = id });
            var player = await connection.QueryFirstOrDefaultAsync<PlayerDetail>(procedure, parameters, commandType: CommandType.StoredProcedure);
            return player;
        }

        /// <summary>
        /// Sử dụng Procedure vẫn có thể lấy về object có nhiều cấp độ nhưng cần phải thực hiện thủ công
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlayerDetailDto> GetPlayerDetailWithMultipleLevel(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "EXEC GetPlayerDetailWithMultipleLevel @PlayerId";

            // Nhận vào 3 đối tượng PlayerDetailDto, TeamDto, ConfederationDto
            // và thực hiện map dữ liệu từ các cột trả về vào đối tượng PlayerDetailDto
            var player = await connection.QueryAsync<PlayerDetailDto, TeamDto, ConfederationDto, PlayerDetailDto>(sql,
                (player, team, confederation) =>
                {
                    player.Team = team;
                    player.Team.Confederation = confederation;
                    return player;
                },
                new { PlayerId = id },
                // Các cột từ đầu đến trước cột TeamId sẽ được ánh xạ vào đối tượng PlayerDetailDto
                // Các cột từ cột TeamId đến trước cột ConfederationId sẽ được ánh xạ vào đối tượng TeamDto
                // Các cột từ cột ConfederationId đến hết sẽ được ánh xạ vào đối tượng ConfederationDto
                splitOn: "TeamId, ConfederationId"
            );

            return player.FirstOrDefault();
        }

        public async Task<List<Player>> GetPlayers()
        {
            using var connection = context.CreateConnection();
            var players = await connection.QueryAsync<Player>("SELECT *FROM Players");
            return players.ToList();
        }

        public async Task<Player> GetPlayer(int id)
        {
            using var connection = context.CreateConnection();
            var player = await connection.QueryFirstOrDefaultAsync<Player>("SELECT * FROM Players WHERE Id=@Id", new { Id = id });
            return player;
        }

        public async Task<List<Player>> GetPlayersWithTeam()
        {
            using var connection = context.CreateConnection();

            // Thực hiện query với 2 bảng Players và Teams, sau đó map dữ liệu vào đối tượng Player
            // Dapper cung cấp một cách để ánh xạ từng hàng trong kết quả trả về thành hai đối tượng:
            // Player: chứa dữ liệu từ các cột thuộc bảng Players.
            // Team: chứa dữ liệu từ các cột thuộc bảng Teams.
            // splitOn: "TeamId" là tên cột mà Dapper sẽ sử dụng để phân chia dữ liệu sau khi join. 
            // Phần dữ liệu trước cột này sẽ được ánh xạ vào đối tượng Player, phần dữ liệu từ cột này về sau sẽ được ánh xạ vào đối tượng Team.
            var players = await connection.QueryAsync<Player, Team, Player>(
                "SELECT p.*, t.Id, t.CountryName as Name, t.ConfederationId FROM Players p INNER JOIN Teams t on p.TeamId = t.Id",
                (player, team) =>
                {
                    player.Team = team;
                    player.TeamId = team.Id;
                    return player;
                },
                splitOn: "TeamId"
            );

            return players.ToList();
        }

        public async Task<bool> CreatePlayer(PlayerCreateUpdateDto player)
        {
            using var connection = context.CreateConnection();

            var parameters = new DynamicParameters(new
            {
                playerName = player.Name,
                teamId = player.TeamId
            });

            var affectedRows = await connection.ExecuteAsync("INSERT INTO Players (Name, TeamId) values (@playerName, @teamId)", parameters);
            // var affectedRows = await connection.ExecuteAsync("INSERT INTO Players (Name, TeamId) values (@Name, @TeamId)", player);

            return affectedRows > 0;
        }

        public async Task<bool> DeletePlayer(int id)
        {
            using var connection = context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync("DELETE FROM Players Where Id = @Id", new { Id = id });

            return affectedRows > 0;
        }

        public async Task<bool> UpdatePlayer(int id, PlayerCreateUpdateDto player)
        {
            using var connection = context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "UPDATE Players SET Name = @Name, TeamId = @TeamId Where Id = @Id",
                new { Id = id, player.Name, player.TeamId }
            );

            return affectedRows > 0;
        }

        /// <summary>
        /// Update multiple players with different values in a single query
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateMultiplePlayers()
        {
            using var connection = context.CreateConnection();
            var updatedPlayerIds = new List<int> { 1, 2, 6, 9 };
            var randomPlayerName = new List<string> { "Messi", "Ronaldo", "Neymar", "Mbappe" };

            var sql = "Update Players Set Name = CASE ";
            var parameters = new DynamicParameters();

            for (var i = 0; i < updatedPlayerIds.Count; i++)
            {
                sql += $"WHEN Id = @Id{updatedPlayerIds[i]} THEN @Name{updatedPlayerIds[i]} ";

                parameters.Add($"Id{updatedPlayerIds[i]}", updatedPlayerIds[i]);
                parameters.Add($"Name{updatedPlayerIds[i]}", randomPlayerName[i]);
            }

            sql += "END WHERE Id IN @Ids";
            parameters.Add("Ids", updatedPlayerIds);

            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }
    }
}
using System.Text.Json.Serialization;

namespace LearnDapper.Repositories.TeamRepo.Schemas
{
    public class TeamDetail
    {
        [JsonPropertyName("id")]
        public int TeamId { get; set; }

        [JsonPropertyName("name")]
        public string TeamName { get; set; }

        public List<PlayerDto> Players { get; set; } = [];
    }

    public class PlayerDto
    {
        [JsonPropertyName("id")]
        public int PlayerId { get; set; }

        [JsonPropertyName("name")]
        public string PlayerName { get; set; }
    }
}
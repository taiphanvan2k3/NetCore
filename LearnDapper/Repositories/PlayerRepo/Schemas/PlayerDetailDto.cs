namespace LearnDapper.Repositories.PlayerRepo.Schemas
{
    public class PlayerDetailDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TeamDto Team { get; set; }
    }

    public class TeamDto
    {
        public int TeamId { get; set; }

        public string Name { get; set; }

        public ConfederationDto Confederation { get; set; }
    }

    public class ConfederationDto
    {
        public int ConfederationId { get; set; }

        public string Name { get; set; }
    }
}
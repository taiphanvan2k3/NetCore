namespace LearnDapper.Databases.Schemas
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ConfederationId { get; set; }

        public virtual Confederation Confederation { get; set; }

        public virtual ICollection<Player> Players { get; set; } = [];
    }
}
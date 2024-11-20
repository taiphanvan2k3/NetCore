namespace LearnDapper.Repositories.PlayerRepo.Schemas
{
    public class PlayerCreateUpdateDto
    {
        public string Name { get; set; }
        public int? TeamId { get; set; }
    }
}
namespace ReviewWebAPI.Models.Schemas
{
    public class RefreshToken
    {
        public string Token { get; set; }

        public DateTime Created { get; set; }

        public DateTime Expires { get; set; }
    }
}
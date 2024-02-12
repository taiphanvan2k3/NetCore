using System.ComponentModel.DataAnnotations;

namespace ReviewWebAPI.Models.Schemas
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
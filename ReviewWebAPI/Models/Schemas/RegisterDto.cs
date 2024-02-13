using System.ComponentModel.DataAnnotations;

namespace ReviewWebAPI.Models.Schemas
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
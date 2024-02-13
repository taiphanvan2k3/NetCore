using System.ComponentModel.DataAnnotations;

namespace ReviewWebAPI.Models.Schemas
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }
    }
}
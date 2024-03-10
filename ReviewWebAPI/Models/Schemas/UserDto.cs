using System.ComponentModel.DataAnnotations;

namespace ReviewWebAPI.Models.Schemas
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Role { get; set; }

        public DateTime BirthDay { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public string SchoolName { get; set; }

        public string Token { get; set; }

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }
    }
}
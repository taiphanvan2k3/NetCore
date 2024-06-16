using System.ComponentModel.DataAnnotations;

namespace RedisCaching.Models
{
    public class Driver
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public int DriverNumber { get; set; }
    }
}
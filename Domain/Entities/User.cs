using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }

        public required string Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public List<Appointment>? Appointments { get; set; }
    }
}

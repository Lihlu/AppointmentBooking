using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class Room
    {
        [Key]
        public required int Id { get; set; }
        [Required]
        public required string Type { get; set; }
    }
}

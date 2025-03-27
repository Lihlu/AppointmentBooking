using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class Appointment
    {
        [Key]
        public required int Id { get; set; }
        [Required]
        public required DateTime startDate { get; set; }
        [Required]
        public DateTime endDate { get; set; }
        [Required]
        public required DateTime bookingDate { get; set; }

        public required int UserId { get; set; }


    }
}

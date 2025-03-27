using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class AppointmentsDto
    {
        [Required]
        public required DateTime startDate { get; set; }
        [Required]
        public DateTime endDate { get; set; }
    }
}

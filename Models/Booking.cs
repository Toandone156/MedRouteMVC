using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedRoute.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public DateTime Date { get; set; }
        public int Order { get; set; }

        //Relations
        public virtual ICollection<MedicalRecord>? MedicalRecords { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedRoute.Models
{
    public class MedicalRecord
    {
        [Key]
        public int MedicalRecordId { get; set; }
        [Column(TypeName = "ntext")]
        public string? MedicalDetail { get; set; }
        [Column(TypeName = "ntext")]
        public string? MedicalResult { get; set; }
        public int BookingOrder { get; set; }
        [StringLength(10)]
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        //ForeignKey
        public int? PatientId { get; set; }
        public int? ServeUserId { get; set; }
        public int BookingId { get; set; }

        //Relations
        [ForeignKey("PatientId")]
        public virtual User? Patient { get; set; }
        [ForeignKey("ServeUserId")]
        public virtual User? ServeUser { get; set; }
        [ForeignKey("BookingId")]
        public virtual Booking? Booking { get; set; }
    }
}

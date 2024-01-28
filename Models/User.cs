using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedRoute.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }
        public string? HashPassword { get; set; }
        [DataType(DataType.EmailAddress)]
        
        //Information
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Full name")]
        public string FullName { get; set; }
        [StringLength(12)]
        public string? CitizenIdentificationCardNumber { get; set; }
		[StringLength(16)]
		public string? InsuranceCode { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string Nation { get; set; } = "Viet nam";

        //ForeignKey
        public int RoleId { get; set; }

        //Relations
        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
        public virtual ICollection<MedicalRecord>? PatientRecords { get; set; }
        public virtual ICollection<MedicalRecord>? ServeRecords { get; set; }
    }
}

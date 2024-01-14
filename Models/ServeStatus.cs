using System.ComponentModel.DataAnnotations;

namespace MedRoute.Models
{
    public class ServeStatus
    {
        [Key]
        public int ServeStatusId { get; set; }
        public string ServeStatusName { get; set; }

        //Relations
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedRoute.Models
{
    // class này dùng để gửi yêu cầu form booking
    [Keyless]
    public class BookingForm
    {
        public string bookingIDStr {  get; set; }
        public int BookingId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? InsuranceCode { get; set; }
        public string MedicalDetail { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedRoute.Models
{
    public class Login
    {
        [Required]
        [StringLength(100)]
        [RegularExpression("^(?:[\\w.-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}|[\\w]+)$", ErrorMessage = "Username/Email is not valid")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class Register
    {
        [Required]
        [StringLength(100)]
        [Remote("ValidateExistUsername", "Auth")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password is not match")]
        [DisplayName("Again password")]
        public string AgainPassword { get; set; }
        [Required]
        [StringLength(255)]
        [DisplayName("Full name")]
		public string FullName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Remote("ValidateExistEmail", "Auth")]
        public string? Email { get; set; }
        [DisplayName("Role")]
        public int? RoleId { get; set; }
    }
}

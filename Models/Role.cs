using System.ComponentModel.DataAnnotations;

namespace MedRoute.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        //Relations
        public virtual ICollection<User>? Accounts { get; set; }
    }
}

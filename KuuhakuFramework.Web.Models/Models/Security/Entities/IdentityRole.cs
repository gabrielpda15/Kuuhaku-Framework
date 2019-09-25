using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KuuhakuFramework.Web.Models.Security.Entities
{
    [Table("Security_UserRole")]
    public class IdentityRole : IdentityUserRole<int>
    {
        public override int UserId { get; set; }
        public override int RoleId { get; set; }
        public virtual Identity User { get; set; }
        public virtual Role Role { get; set; }
    }
}

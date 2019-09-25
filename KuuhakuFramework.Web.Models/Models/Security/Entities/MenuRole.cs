using System.ComponentModel.DataAnnotations.Schema;

namespace KuuhakuFramework.Web.Models.Security.Entities
{
    [Table("Security_MenuRole")]
    public class MenuRole
    {
        public virtual int RoleId { get; set; }
        public virtual int MenuId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Menu Menu { get; set; }
    }
}

using KuuhakuFramework.Web.Models.BaseEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KuuhakuFramework.Web.Models.Security.Entities
{
    [Table("Security_Role")]
    public class Role : IdentityRole<int>, IEntity
    {
        [Key]
        public override int Id { get; set; }

        [DataType("varchar")]
        [StringLength(50)]
        [ScaffoldColumn(false)]
        [Display(Name = "Usuário de Criação")]
        public string CreationUser { get; set; }

        [StringLength(50)]
        [DataType("varchar")]
        [ScaffoldColumn(false)]
        [Display(Name = "Usuário de Edição")]
        public string EditionUser { get; set; }

        [StringLength(50)]
        [DataType("varchar")]
        [ScaffoldColumn(false)]
        [Display(Name = "IP de Criação")]
        public string CreationIp { get; set; }

        [StringLength(50)]
        [DataType("varchar")]
        [ScaffoldColumn(false)]
        [Display(Name = "IP de Edição")]
        public string EditionIp { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Data de Criação")]
        public DateTime? CreationDate { get; set; }

        [ScaffoldColumn(false)]
        [ConcurrencyCheck]
        [Display(Name = "Data de Edição")]
        public DateTime? EditionDate { get; set; }

        [ScaffoldColumn(false)]
        public virtual IList<MenuRole> MenuRoles { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<IdentityRole> Identities { get; set; }
    }
}

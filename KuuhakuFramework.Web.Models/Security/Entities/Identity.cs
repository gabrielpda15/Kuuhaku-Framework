using KuuhakuFramework.Web.Models.BaseEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KuuhakuFramework.Web.Models.Security.Entities
{
    [Table("Security_IdentityUser")]
    public class Identity : IdentityUser<int>, IEntity
    {
        [Key]
        public override int Id { get; set; }

        [Display(Name = "Nome do Usuário")]
        [Required(ErrorMessage = "Nome do Usuário é um campo obrigatório")]
        [StringLength(50, ErrorMessage = "Nome do usuário deve ter no máximo 50 caracteres")]
        public override string UserName { get; set; }

        [NotMapped]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email é um campo obrigatório")]
        [Display(Name = "Email")]
        public override string Email { get; set; }

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
    }
}

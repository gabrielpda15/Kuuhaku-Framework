using KuuhakuFramework.Web.Models.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KuuhakuFramework.Web.Models.Security.Entities
{
    [Table("Security_Menu")]
    public class Menu : BaseEntityDescription
    {
        [Required(ErrorMessage = "Url é um campo requerido!")]
        [DataType("varchar")]
        [StringLength(100, ErrorMessage = "Menu deve ter no máximo 100 caracteres")]
        [Display(Name = "URL")]
        public string Url { get; set; }

        [Display(Name = "Menu Pai")]
        public virtual Menu Parent { get; set; }

        [Required(ErrorMessage = "Ordem é um campo requerido!")]
        [Display(Name = "Ordem")]
        public int Order { get; set; }

        [Display(Name = "Ativo")]
        public bool Active { get; set; }

        [ScaffoldColumn(false)]
        public virtual IList<Menu> Children { get; set; }
    }
}

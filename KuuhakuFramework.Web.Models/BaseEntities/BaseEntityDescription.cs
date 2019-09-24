using System.ComponentModel.DataAnnotations;

namespace KuuhakuFramework.Web.Models.BaseEntities
{
    public class BaseEntityDescription : BaseEntity
    {
        [Required(ErrorMessage = "Descrição é obrigatório!")]
        [StringLength(255, ErrorMessage = "Descrição deve ter no máximo 255 caracteres")]
        [DataType("varchar")]
        [Display(Name = "Descrição")]
        public virtual string Description { get; set; }
    }
}

using KuuhakuFramework.Web.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.Web.Models.Location
{
    [Table("Location_Country")]
    public class Country : BaseEntity
    {
        [StringLength(120)]
        [DataType("varchar")]
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; }

        [StringLength(2)]
        [DataType("varchar")]
        [Display(Name = "Código")]
        [Required(ErrorMessage = "Código é obrigatório")]
        public string Code { get; set; }

        public virtual IList<Region> Regions { get; set; }
    }
}

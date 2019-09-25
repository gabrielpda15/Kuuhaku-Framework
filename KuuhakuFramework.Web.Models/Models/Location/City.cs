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
    [Table("Location_City")]
    public class City : BaseEntity
    {
        [StringLength(120)]
        [DataType("varchar")]
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; }
        public virtual int RegionId { get; set; }
        public virtual int CountryId { get; set; }
    }
}

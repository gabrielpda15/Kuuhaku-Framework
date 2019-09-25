using System;

namespace KuuhakuFramework.Web.Models.BaseEntities
{
    public interface IEntity
    {
        int Id { get; set; }
        string CreationUser { get; set; }
        string EditionUser { get; set; }
        string CreationIp { get; set; }
        string EditionIp { get; set; }
        DateTime? CreationDate { get; set; }
        DateTime? EditionDate { get; set; }
    }
}

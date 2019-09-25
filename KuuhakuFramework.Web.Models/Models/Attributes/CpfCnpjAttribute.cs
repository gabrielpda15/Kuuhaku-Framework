using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KuuhakuFramework.Web.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CPFAttribute : DataTypeAttribute
    {
        public CPFAttribute() : base(DataType.Custom) { }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CNPJAttribute : DataTypeAttribute
    {
        public CNPJAttribute() : base(DataType.Custom) { }
    }
}

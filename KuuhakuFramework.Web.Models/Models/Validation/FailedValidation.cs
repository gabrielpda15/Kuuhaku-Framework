using System;
using System.Collections.Generic;
using System.Text;

namespace KuuhakuFramework.Web.Models.Validation
{
    public class FailedValidation
    {
        public string Field { get; set; }
        public string Validation { get; set; }
        public string Message { get; set; }
    }
}

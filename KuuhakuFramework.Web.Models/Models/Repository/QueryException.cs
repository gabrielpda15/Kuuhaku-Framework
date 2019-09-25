using System;
using System.Collections.Generic;
using System.Text;

namespace KuuhakuFramework.Web.Models.Repository
{
    public class QueryException : Exception
    {
        public QueryException() : base("Não foi possivel localizar apenas um valor que satifaz a condição") { }
        public QueryException(string message) : base(message) { }
        public QueryException(string message, Exception innerException) : base(message, innerException) { }
    }
}

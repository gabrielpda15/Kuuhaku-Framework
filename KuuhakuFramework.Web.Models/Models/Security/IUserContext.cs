using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace KuuhakuFramework.Web.Models.Security
{
    public interface IUserContext
    {
        IPrincipal Principal { get; set; }
        string IP { get; set; }
        string HostName { get; set; }
        IEnumerable<string> Roles { get; set; }
        IEnumerable<Claim> Claims { get; set; }
    }
}

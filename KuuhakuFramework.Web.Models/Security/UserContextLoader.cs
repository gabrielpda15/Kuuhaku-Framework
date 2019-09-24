using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace KuuhakuFramework.Web.Models.Security
{
    public sealed class UserContextLoader : IUserContextLoader
    {
        private IHttpContextAccessor ContextAccessor { get; }

        public UserContextLoader(IHttpContextAccessor accessor)
        {
            this.ContextAccessor = accessor;
        }

        public void Load(IUserContext userContext)
        {
            try
            {
                HttpContext httpContext = this.ContextAccessor.HttpContext;
                if (httpContext == null)
                    return;
                userContext.Principal = httpContext.User;
                if (httpContext.Request != null)
                {
                    userContext.IP = httpContext.Connection.RemoteIpAddress.ToString();
                    userContext.HostName = httpContext.Connection.RemoteIpAddress.ToString();
                    userContext.Roles = httpContext.User.Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Select(x => x.Value);
                    userContext.Claims = httpContext.User.Claims;
                }
            }
            catch { }
        }
    }
}

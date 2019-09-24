namespace KuuhakuFramework.Web.Models.Security
{
    public interface IUserContextLoader
    {
        void Load(IUserContext userContext);
    }
}

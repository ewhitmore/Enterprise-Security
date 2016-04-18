using NHibernate;

namespace Enterprise.Web.Services
{
    public interface ISecurityService
    {
        ISession Session { get; set; }
        void CreateUser();
    }
}
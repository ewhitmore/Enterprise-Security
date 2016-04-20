using NHibernate;

namespace Enterprise.Web.Services
{
    public interface ISecurityService
    {
        ISession Session { get; set; }
        void CreateUser(string email, string username, string password);
        void InitalizeSecurity();
        bool UserExists(string username);
    }
}
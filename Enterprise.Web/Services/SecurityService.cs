using System.Web.UI.WebControls.WebParts;
using Enterprise.Persistence;
using Enterprise.Persistence.Dao;
using Enterprise.Persistence.Model;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.AspNet.Identity;


namespace Enterprise.Web.Services
{
    public class SecurityService : ISecurityService
    {

        public ISession Session { get; set; }

        public void CreateUser()
        {


            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Session));
           
            var user = new ApplicationUser();
            user.Email = "eric@whitmore.com";
            user.UserName = "ewhitmore";



            userManager.Create(user);


        }

    }
}
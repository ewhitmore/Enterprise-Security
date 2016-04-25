using NHibernate.AspNet.Identity;

namespace Enterprise.Persistence.Model
{
    public class ApplicationUser : IdentityUser
    {
        public virtual string Password { get; set; }
    }
}

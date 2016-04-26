using Enterprise.Model;
using FluentNHibernate.Mapping;

namespace Enterprise.Persistence.Dao.Mapping
{
    public class RefreshTokenMap : EntityBaseMap<RefreshToken>
    {
        public RefreshTokenMap()
        {
  
            Map(token => token.ReferenceId);
            Map(token => token.ClientId);
            Map(token => token.ExpiresUtc);
            Map(token => token.IssuedUtc);
            Map(token => token.ProtectedTicket).Length(4001); // anything over 4000 is nvarchar(max)
            Map(token => token.Subject);
      
        }
    }
}

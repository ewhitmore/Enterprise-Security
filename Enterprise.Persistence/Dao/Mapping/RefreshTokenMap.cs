using Enterprise.Model;
using FluentNHibernate.Mapping;

namespace Enterprise.Persistence.Dao.Mapping
{
    public class RefreshTokenMap : ClassMap<RefreshToken>
    {
        public RefreshTokenMap()
        {
            //Id(token => token.Id).GeneratedBy.GuidComb();
            Id(token => token.Id);
            Map(token => token.ClientId);
            Map(token => token.ExpiresUtc);
            Map(token => token.IssuedUtc);
            Map(token => token.ProtectedTicket).Length(4001); // anything over 4000 is nvarchar(max)
            Map(token => token.Subject);
      
        }
    }
}

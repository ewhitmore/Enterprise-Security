using Enterprise.Model;
using FluentNHibernate.Mapping;

namespace Enterprise.Persistence.Dao.Mapping
{
    public class ClientMap : EntityBaseMap<Client>
    {
        public ClientMap()
        {
            Map(client => client.ClientId);
            Map(client => client.Active);
            Map(client => client.AllowedOrigin);
            Map(client => client.ApplicationType);
            Map(client => client.Name);
            Map(client => client.RefreshTokenLifeTime);
            Map(client => client.Secret);

        }
    }
}

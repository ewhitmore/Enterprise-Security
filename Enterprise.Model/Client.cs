using System;

namespace Enterprise.Model
{
    public class Client : EntityBase<Client>
    {
        public virtual string ClientId { get; set; }
        public virtual string Secret { get; set; }
        public virtual string Name { get; set; }
        public virtual ApplicationTypes ApplicationType { get; set; }
        public virtual bool Active { get; set; }
        public virtual int RefreshTokenLifeTime { get; set; }
        public virtual string AllowedOrigin { get; set; }
    }
}

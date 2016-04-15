using Enterprise.Model;
using FluentNHibernate.Mapping;

namespace Enterprise.Persistence.Dao.Mapping
{
    public abstract class EntityBaseMap<T> : ClassMap<T> where T : EntityBase<T>
    {
        protected EntityBaseMap()
        {
            DynamicUpdate(); // Hibernate will update the modified columns only.
            Id(x => x.Id).Column("Id"); // Numberic Id
            Map(x => x.IsDeleted);
            // Id(x => x.Id).Column("Id").GeneratedBy.GuidComb(); // Guid Id
            Map(x => x.CreatedAt);
            OptimisticLock.Version();
            Version(x => x.ModifiedAt).CustomType("Timestamp");
        }
    }
}
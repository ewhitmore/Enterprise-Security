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


            // DateTime2 is not a valid SQLITE database type. To use DateTime2 with SQLITE 
            // you need to extend the SQLITE Dialect to support it
            // http://ewhitmor.blogspot.com/2016/04/sqlite-nhibernate-datetime2.html
            Version(x => x.ModifiedAt).Column("ModifiedAt").CustomType("DateTime2");
            
        }
    }
}
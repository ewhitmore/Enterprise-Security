using System.Collections.Generic;
using System.Linq;

namespace Enterprise.Persistence.Dao
{
    public interface IRepository<TEntity, in TEntityId>
    {
        TEntity Get(TEntityId id);
        void Save(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Flush();
        ICollection<TEntity> GetAll();
        IQueryable<TEntity> FindAll();
        IQueryable<TEntity> FindAllPage(int pageNumber, int pageSize);
    }
}

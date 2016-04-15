using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace Enterprise.Persistence.Dao.Implementation
{
    public class Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId>
    {

        public virtual ISession CurrentSession { get; set; }
        public int Order { get; set; }

        /// <summary>
        ///     Get all entities
        /// </summary>
        /// <returns></returns>
        public virtual ICollection<TEntity> GetAll()
        {
            return CurrentSession.CreateCriteria(typeof(TEntity)).List<TEntity>();
        }

        /// <summary>
        ///     find entities
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> FindAll()
        {
            return CurrentSession.Query<TEntity>();
        }


        /// <summary>
        ///     find entities in page sets
        /// </summary>
        /// <param name="pageNumber">One Based Index</param>
        /// <param name="pageSize">One Based Index</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> FindAllPage(int pageNumber, int pageSize)
        {
            return CurrentSession.Query<TEntity>().Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        ///     Get Object by Unique Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity Get(TEntityId id)
        {
            return CurrentSession.Get<TEntity>(id);
        }

        //todo: combine save and update transaction processing into one private method
        /// <summary>
        ///     Creating New Unattached Object
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Save(TEntity entity)
        {

            using (var tx = CurrentSession.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                CurrentSession.Save(entity);

                try
                {
                    tx.Commit();

                }
                catch (Exception)
                {

                    tx.Rollback();
                    throw;
                }
            }

        }

        //todo: combine save and update transaction processing into one private method
        /// <summary>
        ///     Updates an Existing Attached Object
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            using (var tx = CurrentSession.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                CurrentSession.Update(entity);

                try
                {
                    tx.Commit();

                }
                catch (Exception)
                {

                    tx.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        ///     Delete Object
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            CurrentSession.Delete(entity);
        }

        public virtual void Flush()
        {
            CurrentSession.Flush();
        }
    }
}

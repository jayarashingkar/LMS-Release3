using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UAC.LMS.Models;

namespace UAC.LMS.DAL
{
    // comment here
    /// <summary>
    /// Interface to define database operations
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGenericRepository<TEntity>
    {
        IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "");
        TEntity GetByID(object id);
        void Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        void Register(LMSLogin model);
        LMSLogin CheckPassword(LMSLogin model);
        bool CheckIfUserExists(string userName);
        void ChangePassword(LMSLogin model);
    }
}

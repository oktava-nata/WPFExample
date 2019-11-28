using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Infrastructure.Domain;

namespace Infrastructure.EF
{
    public class GenericCUDService<TEntity> : GenericService<TEntity>, ICUDService<TEntity>
        where TEntity : class, IAggregateRootEntity
    {

        protected GenericCUDService(BaseContext unitofwork) : base(unitofwork)
        { }

        #region Actions
        public virtual bool Add(TEntity entity)
        {
            TEntitySet.Add(entity);
            return Context.SaveChanges() > 0;
        }

        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            TEntitySet.Add(entity);
            int result = await Context.SaveChangesAsync();
            return result> 0;
        }

        public virtual bool Update(TEntity entity)
        {
            Attach(entity);
            Entry(entity).State = EntityState.Modified;
            return Context.SaveChanges() > 0;
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            Attach(entity);
            Entry(entity).State = EntityState.Modified;
            int result = await Context.SaveChangesAsync();
            return result > 0;
        }

        public virtual bool Delete(TEntity entity)
        {
            Attach(entity);
            TEntitySet.Remove(entity);
            return Context.SaveChanges() > 0;
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            Attach(entity);
            TEntitySet.Remove(entity);
            int result = await Context.SaveChangesAsync();
            return result > 0;
        }
        #endregion

    }
}

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure.Domain;

namespace Infrastructure.EF
{
    public class GenericService<TEntity>: IDisposable
        where TEntity : class, IAggregateRootEntity
    {
        public DbSet<TEntity> TEntitySet => Context.Set<TEntity>();
        public BaseContext Context { get; private set; }

        public GenericService(BaseContext context)
        {
            Context = context ?? throw new ArgumentNullException("Context argument type BaseContext is null!");
        }

        public IQueryable<TEntity> GetByCoditionIncludes(ISpecification<TEntity> filterCondition = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = TEntitySet;
            if (includeProperties != null)
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            if (filterCondition != null)
                query = query.Where(filterCondition.ToExpression());

            return query;
        }

        public bool Attach(TEntity entity)
        {
            if (Entry(entity).State == EntityState.Detached)
            {
                Context.Set<TEntity>().Attach(entity);
                return true;
            }
            return false;
        }

        public DbEntityEntry<TEntity> Entry(TEntity entity)
        {
            return Context.Entry(entity);
        }

        public void Dispose()
        {
            var list = Context.ChangeTracker.Entries();
            foreach (var item in list)
                item.State = EntityState.Detached;
            Context.Dispose();
            Context = null;
        }
    }
}

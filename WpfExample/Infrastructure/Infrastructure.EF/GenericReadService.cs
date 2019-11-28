using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Infrastructure.Domain;

namespace Infrastructure.EF
{
    public abstract class GenericReadService<TEntity, TContext> : IReadService<TEntity>
        where TEntity : class, IAggregateRootEntity
        where TContext : BaseContext
    {
        protected abstract TContext CreateContext();

        protected GenericReadService()
        { }

        #region Get by Id methods

        public TEntity GetById(int id)
        {
            return GetById(id, null);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await GetByIdAsync(id, null);
        }

        public TEntity GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            GenericService<TEntity> service = new GenericService<TEntity>(CreateContext());
            var e = service.GetByCoditionIncludes(GetSpecificationForGetById(id), includeProperties).AsNoTracking().SingleOrDefault();
            service.Dispose();
            return e;
        }

        public async Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            GenericService<TEntity> service = new GenericService<TEntity>(CreateContext());
            var e = await service.GetByCoditionIncludes(GetSpecificationForGetById(id), includeProperties).AsNoTracking().SingleOrDefaultAsync();
            service.Dispose();
            return e;
        }

        protected virtual ISpecification<TEntity> GetSpecificationForGetById(int id)
        {
            return new EntityIdSpecification(id);
        }
        #endregion

        public ICollection<TEntity> GetCollection(ISpecification<TEntity> filterCondition = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            GenericService<TEntity> service;
            var result = GetForRead(out service, filterCondition, includeProperties).ToList();
            service.Dispose();
            return result;
        }

        public async Task<ICollection<TEntity>> GetCollectionAsync(ISpecification<TEntity> filterCondition = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            GenericService<TEntity> service;
            var result = await GetForRead(out service, filterCondition, includeProperties).ToListAsync();
            service.Dispose();
            return result;
        }

        protected IQueryable<TEntity> GetForRead(out GenericService<TEntity> service, ISpecification<TEntity> filterCondition = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            service = new GenericService<TEntity>(CreateContext());
            return service.GetByCoditionIncludes(filterCondition, includeProperties).AsNoTracking();
        }

        //пришлось сделать внутренним классом, что бы избежать приведение типов на уровне EF (из IEntity), т.к. EF делать это не умеет => ошибка
        class EntityIdSpecification : Infrastructure.EF.Specification<TEntity>
        {
            public EntityIdSpecification(int id)
            {
                this.Predicate = i => i.Id == id;
            }
        }

    }
}

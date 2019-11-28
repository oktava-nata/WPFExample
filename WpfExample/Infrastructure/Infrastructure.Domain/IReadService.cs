using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{

    public interface IReadService<TEntity> where TEntity : IAggregateRootEntity
    {
        ICollection<TEntity> GetCollection(ISpecification<TEntity> filterCondition = null, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<ICollection<TEntity>> GetCollectionAsync(ISpecification<TEntity> filterCondition = null, params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}

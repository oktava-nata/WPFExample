using System;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public interface ICUDService<TEntity> : IDisposable
        where TEntity : IAggregateRootEntity
    {
        bool Add(TEntity entity);
        Task<bool> AddAsync(TEntity entity);

        bool Update(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);

        bool Delete(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
    }
}

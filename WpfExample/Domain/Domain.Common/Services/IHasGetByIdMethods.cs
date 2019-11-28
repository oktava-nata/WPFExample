using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Domain;

namespace Domain.Common.Services
{
    public interface IHasGetByIdMethods<TEntity> where TEntity : IAggregateRootEntity
    {
        TEntity GetById(int id);
        Task<TEntity> GetByIdAsync(int id);
    }
}

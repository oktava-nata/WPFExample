using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Domain;

namespace Domain.Common.Services
{
    public interface IHasGetAllMethods<TEntity> where TEntity : IAggregateRootEntity
    {
        Task<ICollection<TEntity>> GetAllAsync();
        ICollection<TEntity> GetAll();
    }
}

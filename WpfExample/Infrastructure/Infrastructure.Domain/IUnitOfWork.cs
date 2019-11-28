using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}

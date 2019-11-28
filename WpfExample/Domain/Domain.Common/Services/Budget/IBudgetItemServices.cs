using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Domain;
using Models.Budget;

namespace Domain.Common.Services.Budget
{
    public interface IBudgetItemReadService : IHasGetByIdMethods<BudgetItem>
    {
        Task<ICollection<BudgetItem>> GetAllByShipownerAsync(int shipownerId);
        ICollection<BudgetItem> GetAllByShipowner(int shipownerId);
        Task<ICollection<BudgetItem>> GetAllForCompanyAsync();
        ICollection<BudgetItem> GetAllForCompany();
    }

    public interface IBudgetItemCUDService : ICUDService<BudgetItem>
    {
    }

    public interface IBudgetItemCopyService
    {
        BudgetItem CopyBudgetItemWithChildrenItemsToShipowner(int itemId, int shipownerId);
        Task<BudgetItem> CopyBudgetItemWithChildrenItemsToShipownerAsync(int itemId, int shipownerId);
    }
}

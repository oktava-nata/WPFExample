using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Common.Services.Budget;
using Domain.EFContexts.Contexts;
using Domain.EFContexts.Factories;
using Domain.Services.Specifications.Budget;
using Infrastructure.EF;
using Models.Budget;

namespace Domain.Services.Budget
{

    internal class BudgetItemReadService : GenericReadService<BudgetItem, BudgetContext>, IBudgetItemReadService
    {
        protected override BudgetContext CreateContext()
        {
            return new BudgetUnitOfWorkFactory().CreateBudgetContext();
        }

        public BudgetItemReadService() : base()
        {
        }

        public ICollection<BudgetItem> GetAllByShipowner(int shipownerId)
        {
            return base.GetCollection(new BudgetItemsByShipownerSpecification(shipownerId));
        }

        public async Task<ICollection<BudgetItem>> GetAllByShipownerAsync(int shipownerId)
        {
            return await base.GetCollectionAsync(new BudgetItemsByShipownerSpecification(shipownerId));
        }

        public ICollection<BudgetItem> GetAllForCompany()
        {
            return base.GetCollection(new BudgetItemsByShipownerSpecification(null));
        }

        public async Task<ICollection<BudgetItem>> GetAllForCompanyAsync()
        {
            return await base.GetCollectionAsync(new BudgetItemsByShipownerSpecification(null));
        }

        
    }
}

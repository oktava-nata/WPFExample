using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Common.Services.Budget;
using Domain.EFContexts.Contexts;
using Domain.EFContexts.Factories;
using Infrastructure.EF;
using Models.Budget;

namespace Domain.Services.Budget
{

    internal class CurrencyReadService : GenericReadService<Currency, BudgetContext>, ICurrencyReadService
    {
        protected override BudgetContext CreateContext()
        {
            return new BudgetUnitOfWorkFactory().CreateBudgetContext();
        }

        public CurrencyReadService() : base()
        {
        } 

        public async Task<ICollection<Currency>> GetAllAsync()
        {
            return await base.GetCollectionAsync();
        }

        public ICollection<Currency> GetAll()
        {
            return base.GetCollection();
        }
    }
}

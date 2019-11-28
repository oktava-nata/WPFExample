using Domain.Common.Services.Budget;
using Domain.EFContexts.Contexts;
using Domain.EFContexts.Factories;
using Infrastructure.EF;
using Models.Budget;

namespace Domain.Services.Budget
{

    internal class CurrencyCUDService : GenericCUDService<Currency>, ICurrencyCUDService
    {

        internal CurrencyCUDService(BudgetContext unitofwork) : base(unitofwork)
        {
        }

        public CurrencyCUDService() : base(new BudgetUnitOfWorkFactory().CreateBudgetContext())
        {
        }

    }
}

using Domain.Common.Services.Budget;
using Domain.EFContexts.Contexts;
using Domain.EFContexts.Factories;
using Infrastructure.EF;
using Models.Budget;

namespace Domain.Services.Budget
{

    internal class BudgetItemCUDService : GenericCUDService<BudgetItem>, IBudgetItemCUDService
    {

        internal BudgetItemCUDService(BudgetContext unitofwork) : base(unitofwork)
        {
        }

        public BudgetItemCUDService() : base(new BudgetUnitOfWorkFactory().CreateBudgetContext())
        {
        }

    }
}

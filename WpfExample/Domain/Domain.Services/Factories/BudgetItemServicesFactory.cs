using Domain.Common.Services.Budget;
using Domain.Services.Budget;

namespace Domain.Services.Factories
{
    public static class BudgetItemServicesFactory
    {

        public static IBudgetItemReadService CreateBudgetItemReadService()
        {
            return new BudgetItemReadService();
        }

        public static IBudgetItemCUDService CreateBudgetItemCUDService()
        {
            return new BudgetItemCUDService();
        }

        public static IBudgetItemCopyService CreateBudgetItemCopyService()
        {
            return new BudgetItemCopyService();
        }
    }
}

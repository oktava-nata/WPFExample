using System;
using System.Threading.Tasks;
using BUDGETModule.ViewModels.Directories.BudgetItems;
using Infrastructure.Domain;
using Models.Budget;
using SharedModule.ViewModels.Directories;
using VMBaseSolutions.VMServices;

namespace BUDGETModule.VMServices
{
    class BudgetItemVMService : CUDVMService<BudgetItemViewModel, BudgetItem>
    {
        protected override ICUDService<BudgetItem> CreateCUDService()
        {
            return Domain.Services.Factories.BudgetItemServicesFactory.CreateBudgetItemCUDService();
        }

        public bool CopyWithChildrenItemsToShipOwner(BudgetItemViewModel budgetItem, ShipOwnerViewModel shipOwner)
        {
            if (budgetItem == null) throw new ArgumentNullException("Arg budgetItem is null!");
            if (shipOwner == null) throw new ArgumentNullException("Arg shipOwner is null!");
            if (budgetItem.Id <= 0) throw new ArgumentOutOfRangeException("Arg budgetItem.Id not more zero!");
            if (shipOwner.Id <= 0) throw new ArgumentOutOfRangeException("Arg shipOwner.Id not more zero!");

            var service = Domain.Services.Factories.BudgetItemServicesFactory.CreateBudgetItemCopyService();
            BudgetItem copy = service.CopyBudgetItemWithChildrenItemsToShipowner(budgetItem.Id, shipOwner.Id);
            return (copy != null);

        }

        public async Task<bool> CopyWithChildrenItemsToShipOwnerAsync(BudgetItemViewModel budgetItem, ShipOwnerViewModel shipOwner)
        {          
            if (budgetItem == null) throw new ArgumentNullException("Arg budgetItem is null!");
            if (shipOwner == null) throw new ArgumentNullException("Arg shipOwner is null!");
            if (budgetItem.Id <= 0) throw new ArgumentOutOfRangeException("Arg budgetItem.Id not more zero!");
            if (shipOwner.Id <= 0) throw new ArgumentOutOfRangeException("Arg shipOwner.Id not more zero!");

            var service = Domain.Services.Factories.BudgetItemServicesFactory.CreateBudgetItemCopyService();
            BudgetItem copy = await service.CopyBudgetItemWithChildrenItemsToShipownerAsync(budgetItem.Id, shipOwner.Id);
            return (copy != null);

        }


    }
}

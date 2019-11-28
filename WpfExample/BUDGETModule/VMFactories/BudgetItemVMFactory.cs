using System;
using System.Threading.Tasks;
using BUDGETModule.ViewModels.Directories.BudgetItems;
using Domain.Common.Services.Budget;
using SharedModule.ViewModels.Directories;
using VMBaseSolutions.VMFactories;

namespace BUDGETModule.VMFactories
{
    class BudgetItemVMFactory: IVMFactory<BudgetItemViewModel>
    {
        IBudgetItemReadService _Service;

        public BudgetItemVMFactory()
        {
            _Service = Domain.Services.Factories.BudgetItemServicesFactory.CreateBudgetItemReadService();
        }

        public async Task<BudgetItemViewModel> GetByIdAsync(int itemId)
        {
            var entity = await _Service.GetByIdAsync(itemId);
            return new BudgetItemViewModel(entity);
        }
        public BudgetItemViewModel GetById(int itemId)
        {
            var entity = _Service.GetById(itemId);
            return new BudgetItemViewModel(entity);
        }

        public BudgetItemViewModel Create()
        {
            Models.Budget.BudgetItem item = new Models.Budget.BudgetItem();
            item.IsInBudgetPlanByDefault = true;
            return new BudgetItemViewModel(item);
        }

        public BudgetItemViewModel CreateForShipOwner(ShipOwnerViewModel shipOwner)
        {
            if(shipOwner == null) throw new ArgumentNullException("shipOwner is null or empty!");
            var item = Create();
            item.GetEntity().SetShipOwner(shipOwner.GetEntity());
            return item;
        }


    }
}

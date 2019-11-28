using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BUDGETModule.VMFactories;

namespace BUDGETModule.ViewModels.Directories.BudgetItems
{
    class BudgetItemsCompanyViewModel : BudgetItemsBaseViewModel
    {

        public BudgetItemsCompanyViewModel() : base() { }

        protected override BudgetItemViewModel CreateNewBudgetItemVMForAdding()
        {
            return new BudgetItemVMFactory().Create();
        }

        protected override async Task<ObservableCollection<BudgetItemViewModel>> LoadBudgetItemVMListAsync()
        {
            return await _Loader.GetAllBudgetItemVMOutOfShipownerTreeViewAsync();
        }

        protected override ObservableCollection<BudgetItemViewModel> LoadBudgetItemVMList()
        {
            return _Loader.GetAllBudgetItemVMOutOfShipownerTreeView();
        }
    }
}

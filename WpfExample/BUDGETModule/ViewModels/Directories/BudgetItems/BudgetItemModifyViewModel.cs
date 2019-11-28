using System;
using System.Collections.ObjectModel;
using System.Linq;
using BUDGETModule.VMCollectionLoaders;
using BUDGETModule.VMFactories;
using BUDGETModule.VMServices;
using Models.Budget;
using Telerik.Windows.Data;
using VMBaseSolutions.ModifyVMs;

namespace BUDGETModule.ViewModels.Directories.BudgetItems
{
    class BudgetItemViewAndModifyViewModel : ViewAndModifyWithFactoryAndCUDServiceVM<BudgetItemVMFactory, BudgetItemVMService, BudgetItemViewModel, BudgetItem>
    {
        #region Properties  

        public ObservableCollection<BudgetItemViewModel> TreeGroupingBudgetItemList
        {
            get { return _TreeGroupingBudgetItemList; }
            private set { _TreeGroupingBudgetItemList = value; OnPropertyChanged(() => this.TreeGroupingBudgetItemList); }
        }
        ObservableCollection<BudgetItemViewModel> _TreeGroupingBudgetItemList;


        Func<BudgetItemVMCollectionLoader> _getActualBudgetItemLoader;
        Func<BudgetItemViewModel> _createNewBudgetItemForAdding;


        #endregion

        public BudgetItemViewAndModifyViewModel() { }

        public BudgetItemViewAndModifyViewModel(Func<BudgetItemVMCollectionLoader> getActualBudgetItemLoaderMethod, Func<BudgetItemViewModel> createNewBudgetItemForAdding)
        {
            _getActualBudgetItemLoader = getActualBudgetItemLoaderMethod;
            _createNewBudgetItemForAdding = createNewBudgetItemForAdding;
        }

        #region Initialize Actions Methods
        void Initialize_ParentBudgetItemList(int? defaultItemParentId = null)
        {
            var loader = _getActualBudgetItemLoader?.Invoke();

            if (loader != null)
                TreeGroupingBudgetItemList = loader.GetGroupingBudgetItemTreeVMAvailableForChildBudgetItem(ModifyTargetVM);
            else
                TreeGroupingBudgetItemList = null;

            if (ModifyTargetVM != null && TreeGroupingBudgetItemList != null && defaultItemParentId.HasValue)
                ModifyTargetVM.ParentItem = TreeGroupingBudgetItemList.Where(i => i.Id == defaultItemParentId).FirstOrDefault();
        }


        public void Initialize_ActionAdd(int? defaultItemParentId = null)
        {
            base.Initialize_ActionAdd();
            Initialize_ParentBudgetItemList(defaultItemParentId);
        }

        public override void Initialize_ActionEdit(BudgetItemViewModel source)
        {
            base.Initialize_ActionEdit(source);
            Initialize_ParentBudgetItemList(ModifyTargetVM.IdParent);
        }
        #endregion

        #region Ready Methods

        protected override BudgetItemViewModel CreateTViewModelForAdding()
        {
            if (_createNewBudgetItemForAdding == null) return base.CreateTViewModelForAdding();
            else return _createNewBudgetItemForAdding?.Invoke();
        }


        #endregion


    }
}

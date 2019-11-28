using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BUDGETModule.VMCollectionLoaders;
using Models.Budget;
using SharedModule.ViewModels.Directories;
using Telerik.Windows.Controls;
using VMBaseSolutions.CollectionVMs;

namespace BUDGETModule.ViewModels.Directories.BudgetItems
{
    internal abstract class BudgetItemsBaseViewModel : CollectionWithCRUDCommandsAndViewModifyVM<BudgetItemViewAndModifyViewModel, BudgetItemViewModel> 
    {
        #region Properties             
       
        protected BudgetItemVMCollectionLoader _Loader;
        #endregion

        protected abstract Task<ObservableCollection<BudgetItemViewModel>> LoadBudgetItemVMListAsync();
        protected abstract ObservableCollection<BudgetItemViewModel> LoadBudgetItemVMList();
        protected abstract BudgetItemViewModel CreateNewBudgetItemVMForAdding();

        public BudgetItemsBaseViewModel() : base() { }

        protected override BudgetItemViewAndModifyViewModel CreateModifyVM()
        {
            return new BudgetItemViewAndModifyViewModel(delegate() { return _Loader; }, CreateNewBudgetItemVMForAdding) ;
        }

        protected override bool SelectElementInCollection(BudgetItemViewModel selectElement)
        {
            if (_Loader == null || _Loader.BudgetItemVMList == null) throw new Exception("Loader or BudgetItemTreeVMList are not defined!");

            if (selectElement == null)
            {
                CurrentElement = null;
                return true;
            }
            else
            {
                var elementFromCollection = _Loader.BudgetItemVMList.Where(i => i.Id == selectElement.Id).FirstOrDefault();
                CurrentElement = elementFromCollection;
                return elementFromCollection != null;
            }
        }

        protected async override Task<ICollection<BudgetItemViewModel>> LoadCollectionAsync()
        {
            _Loader = new BudgetItemVMCollectionLoader();
            return await LoadBudgetItemVMListAsync();
        }

        protected override ICollection<BudgetItemViewModel> LoadCollection()
        {
            _Loader = new BudgetItemVMCollectionLoader();
            return LoadBudgetItemVMList();
        }

        #region Execute Commands

        protected override void OnAddCommandExecute()
        {
            ModifyAndViewVM.Initialize_ActionAdd((CurrentElement != null) ? (int?)CurrentElement.Id : null);
        }

        protected override void OnDeleteCommandExecute()
        {
            if (CurrentElement == null) return;
            //if (MsgConfirm.Show(Properties.ResourcesMsg.mConfirmDelInspectionName) == MessageBoxResult.No) return;
            //if (_InspectionNameService.DeleteOrSetUnactive(CurrentBudgetItem))
            // Task.Run(() => BudgetItemCollectionInitializeAsync());
        }


        #endregion
    }
}

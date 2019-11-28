using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BUDGETModule.VMFactories;
using Models.Budget;
using SharedModule.ViewModels.Directories;
using SharedModule.VMCollectionLoaders;
using Telerik.Windows.Controls;

namespace BUDGETModule.ViewModels.Directories.BudgetItems
{
    class BudgetItemsFleetViewModel : BudgetItemsBaseViewModel
    {
        #region Commands  
        public DelegateCommand CopyCommand { get; private set; }
        #endregion

        #region Properties  

        public ObservableCollection<ShipOwnerViewModel> ShipOwnerVMList
        {
            get { return _ShipOwnerVMList; }
            private set { _ShipOwnerVMList = value; OnPropertyChanged(() => this.ShipOwnerVMList); }
        }
        ObservableCollection<ShipOwnerViewModel> _ShipOwnerVMList;

        public ShipOwnerViewModel CurrentShipOwner
        {
            get { return selectedShipOwner; }
            set
            {
                if (this.selectedShipOwner != value)
                {
                    this.selectedShipOwner = value;
                    this.OnPropertyChanged(() => this.CurrentShipOwner);
                    ShipOwnerVMList_CurrentChangedAsync();
                }
            }
        }
        private ShipOwnerViewModel selectedShipOwner;

        public BudgetItemCopyViewModel BudgetItemCopyVM
        {
            get { return _BudgetItemCopyVM; }
            private set { _BudgetItemCopyVM = value; OnPropertyChanged(() => this.BudgetItemCopyVM); }
        }
        BudgetItemCopyViewModel _BudgetItemCopyVM;

        ShipOwnerVMCollectionLoader _ShipOwnerLoader;

        #endregion


        public BudgetItemsFleetViewModel() : base() { }


        protected async override Task VMDataInitializeAsync()
        {
            await ShipOwnerAvaibleCollectionInitializeAsync();
            BudgetItemCopyVM = new BudgetItemCopyViewModel(_ShipOwnerLoader);
        }

        protected override void InitializeCommands()
        {
            this.MenuCommandCRUDVM.InitializeAllCommands
                (onAddCommandExecute: OnAddCommandExecute
                , onEditCommandExecute: OnEditCommandExecute
                , onDeleteCommandExecute: OnDeleteCommandExecute
                , canAddCommand: canAddCommandExecuted);
            CopyCommand = new DelegateCommand(OnCopy, CanCopy);
        }

        async Task ShipOwnerAvaibleCollectionInitializeAsync()
        {
            _ShipOwnerLoader = new ShipOwnerVMCollectionLoader();
            //var result = await UIMessager.Services.Execeptions.ExceptionНandling.HandleLoadingActionAsync(_ShipOwnerLoader.GetAllAsync);
            ShipOwnerVMList = await _ShipOwnerLoader.GetAllAsync();//result.ReturnValue;
            if (ShipOwnerVMList != null)
                CurrentShipOwner = ShipOwnerVMList.FirstOrDefault();
        }

        async void ShipOwnerVMList_CurrentChangedAsync()
        {
            CurrentElement = null;
            await CollectionInitializeAsync();
            ModifyAndViewVM.Initialize_ActionView(CurrentElement);
            this.MenuCommandCRUDVM.AddNewCommand.InvalidateCanExecute();
        }

        protected override BudgetItemViewModel CreateNewBudgetItemVMForAdding()
        {
            return new BudgetItemVMFactory().CreateForShipOwner(CurrentShipOwner);
        }

        protected override async Task<ObservableCollection<BudgetItemViewModel>> LoadBudgetItemVMListAsync()
        {
            if (CurrentShipOwner != null)
                return await _Loader.GetAllBudgetItemVMByShipownerTreeViewAsync(CurrentShipOwner.Id);
            return null;
        }

        protected override ObservableCollection<BudgetItemViewModel> LoadBudgetItemVMList()
        {
            if (CurrentShipOwner != null)
                return _Loader.GetAllBudgetItemVMByShipownerTreeView(CurrentShipOwner.Id);
            return null;
        }

        protected override void CurrentElementChanged(bool isSelectedAfterCollectionInitialize)
        {
            CopyCommand.InvalidateCanExecute();
            base.CurrentElementChanged(isSelectedAfterCollectionInitialize);
        }

        #region CanExecute 

        private bool CanCopy(object obj)
        {
            return CurrentElement != null;
        }

        bool canAddCommandExecuted()
        {
            return CurrentShipOwner != null;
        }
        #endregion

        #region Execute Commands

        private void OnCopy(object obj)
        {
            BudgetItemCopyVM.Initialize(CurrentElement);
            BudgetItemCopyVM.IsBudgetItemCopyPanelShow = true;
        }
        #endregion
    }
}

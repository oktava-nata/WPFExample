using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SharedModule.ViewModels.Directories;
using SharedModule.VMCollectionLoaders;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using UIMessager.Services.Message;

namespace BUDGETModule.ViewModels.Directories.BudgetItems
{
    class BudgetItemCopyViewModel : Telerik.Windows.Controls.ViewModelBase
    {
        #region Commands  
        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        #endregion

        #region Properties  
        public BudgetItemViewModel SourceBudgetItem
        {
            get { return _SourceBudgetItem; }
            private set { _SourceBudgetItem = value; OnPropertyChanged(() => this.SourceBudgetItem); }
        }
        BudgetItemViewModel _SourceBudgetItem;

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
                    ShipOwnerVMList_CurrentChanged();
                }
            }
        }
        private ShipOwnerViewModel selectedShipOwner;

        public bool IsBudgetItemCopyPanelShow
        {
            get { return _IsBudgetItemCopyPanelShow; }
            set { _IsBudgetItemCopyPanelShow = value; OnPropertyChanged(() => this.IsBudgetItemCopyPanelShow); }
        }
        bool _IsBudgetItemCopyPanelShow;

        public bool IsCopyProcess
        {
            get { return _IsCopyProcess; }
            private set { _IsCopyProcess = value; OnPropertyChanged(() => this.IsCopyProcess); }
        }
        bool _IsCopyProcess;


        ShipOwnerVMCollectionLoader _ShipOwnerLoader;
        Action<ShipOwnerViewModel> _OnSuccessCoping;
        #endregion

        public BudgetItemCopyViewModel(ShipOwnerVMCollectionLoader shipOwnerLoader, Action<ShipOwnerViewModel> onSuccessCoping = null)
        {
            _ShipOwnerLoader = shipOwnerLoader ?? throw new ArgumentNullException("Arg shipOwnerLoader is null!");
            _OnSuccessCoping = onSuccessCoping;
            CopyCommand = new DelegateCommand(OnCopyCommandExecute, delegate (object obj) { return CurrentShipOwner != null; });
            CancelCommand = new DelegateCommand(OnCancelCommandExecute);
        }

        #region Initialize Methods

        public void Initialize(BudgetItemViewModel sourceBudgetItemVM)
        {
            SourceBudgetItem = sourceBudgetItemVM ?? throw new ArgumentNullException("Arg sourceBudgetItemVM is null!");
            ShipOwnerAvaibleCollectionInitialize();
            CopyCommand.InvalidateCanExecute();
        }

        void ShipOwnerAvaibleCollectionInitialize()
        {
            var list = (SourceBudgetItem.IdShipOwner != null) ? _ShipOwnerLoader.ShipOwnerVMList.Where(i => i.Id != SourceBudgetItem.IdShipOwner)
                : _ShipOwnerLoader.ShipOwnerVMList;
            ShipOwnerVMList = new ObservableCollection<ShipOwnerViewModel>(_ShipOwnerLoader.ShipOwnerVMList.Where(i => i.Id != SourceBudgetItem.IdShipOwner));
        }

        void ShipOwnerVMList_CurrentChanged()
        {
            CopyCommand.InvalidateCanExecute();
        }
        #endregion

        async Task<bool> CopyBudgetItemAsync()
        {
            return await new VMServices.BudgetItemVMService().CopyWithChildrenItemsToShipOwnerAsync(SourceBudgetItem, CurrentShipOwner);
        }

        bool CopyBudgetItem()
        {
            return new VMServices.BudgetItemVMService().CopyWithChildrenItemsToShipOwner(SourceBudgetItem, CurrentShipOwner);
        }

        #region Command Execute
        async void OnCopyCommandExecute(object obj)
        {
            IsBudgetItemCopyPanelShow = false;
            IsCopyProcess = true;
            var result = await UIMessager.Services.Execeptions.ExceptionНandling.HandleBaseActionAsync<bool>(CopyBudgetItemAsync, UIMessager.Services.Message.MsgError.BaseType.Copying);
            IsCopyProcess = false;
            if (result.IsSuccess)
            {
                InfoMessage.Show(global::Resources.Properties.Resources.mOkCoping, null);
                _OnSuccessCoping?.Invoke(CurrentShipOwner);
            }
        }

        void OnCancelCommandExecute(object obj)
        {
            IsBudgetItemCopyPanelShow = false;
        }
        #endregion
    }
}

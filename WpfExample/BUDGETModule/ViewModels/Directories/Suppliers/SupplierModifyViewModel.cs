using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BUDGETModule.VMFactories;
using BUDGETModule.VMServices;
using Controls;
using Models.Budget;
using SharedModule.ViewModels.Directories;
using SharedModule.VMCollectionLoaders;
using VMBaseSolutions.ModifyVMs;

namespace BUDGETModule.ViewModels.Directories.Suppliers
{
    class SupplierModifyViewModel : ModifyWithFactoryAndCUDServiceVM<SupplierVMFactory, SupplierVMService, SupplierViewModel, Supplier>
    {
        #region Properties  
        public MultiSelectPanelViewModel<ShipOwnerViewModel> ShipOwnerSelectVM
        {
            get { return _ShipOwnerSelectVM; }
            private set { _ShipOwnerSelectVM = value; OnPropertyChanged(() => this.ShipOwnerSelectVM); }
        }
        MultiSelectPanelViewModel<ShipOwnerViewModel> _ShipOwnerSelectVM;

        ShipOwnerVMCollectionLoader _ShipOwnerLoader;
        #endregion

        public SupplierModifyViewModel(ShipOwnerVMCollectionLoader shipOwnerLoader, Action<SupplierViewModel> onChanged) :
            base(onModifyViewModel: onChanged)
        {
            _ShipOwnerLoader = shipOwnerLoader;
        }


        public async override Task Initialize_ActionAddAsync()
        {
            await ShipOwnerAvaibleCollectionInitializeAsync();
            await base.Initialize_ActionAddAsync();
            if (ShipOwnerSelectVM == null || ModifyTargetVM == null) return;
            ShipOwnerSelectVM.Clear();
        }

        //используется при команде Применить
        public override void Initialize_ActionAdd()
        {
            base.Initialize_ActionAdd();
            if (ShipOwnerSelectVM == null || ModifyTargetVM == null) return;
            ShipOwnerSelectVM.Clear();
        }

        public async override Task Initialize_ActionEditAsync(SupplierViewModel source)
        {
            await ShipOwnerAvaibleCollectionInitializeAsync();
            await base.Initialize_ActionEditAsync(source);
            if (ShipOwnerSelectVM == null || ModifyTargetVM == null) return;
            ShipOwnerSelectVM.SetDefaultSelectList(ModifyTargetVM.ShipOwnerVMList.ToList());
        }

        #region TModelChanged
        public override bool WasAddingTModelChanged()
        {
            return base.WasAddingTModelChanged() || (ShipOwnerSelectVM != null) && ShipOwnerSelectVM.SelectedItems.Count > 0;
        }

        public override bool WasEdittingTModelChanged()
        {
            if (ModifyTargetVM == null) return false;
            bool result = base.WasEdittingTModelChanged();
            if (result) return true;
            if (ShipOwnerSelectVM != null && ShipOwnerSelectVM.SelectedItems.Count != ModifyTargetVM.ShipOwnerVMList.Count) return true;
            if (ShipOwnerSelectVM != null && ShipOwnerSelectVM.SelectedItems.Select(i => i.Id).Except(ModifyTargetVM.ShipOwnerVMList.Select(i => i.Id)).Count() > 0) return true;
            if (ModifyTargetVM.ShipOwnerVMList.Select(i => i.Id).Except(ShipOwnerSelectVM.SelectedItems.Select(i => i.Id)).Count() > 0) return true;
            return false;
        }
        #endregion

        async Task ShipOwnerAvaibleCollectionInitializeAsync()
        {
            if (ShipOwnerSelectVM != null) return;
            List<ShipOwnerViewModel> resultList = null;
            if (_ShipOwnerLoader.ShipOwnerVMList != null)
                resultList = _ShipOwnerLoader.ShipOwnerVMList.ToList();
            else
            {
                var result = await UIMessager.Services.Execeptions.ExceptionНandling.HandleLoadingActionAsync(_ShipOwnerLoader.GetAllAsync);
                resultList = (result.IsSuccess) ? result.ReturnValue.ToList() : null;
            }
            if (resultList != null)
                ShipOwnerSelectVM = new MultiSelectPanelViewModel<ShipOwnerViewModel>(resultList, null, null);

        }

        protected override bool AddTViewModel(SupplierViewModel targetVM)
        {
            if (ShipOwnerSelectVM != null && ShipOwnerSelectVM.SelectedItems != null)
                targetVM.UpdateShipOwnerList(ShipOwnerSelectVM.SelectedItems);
            return base.AddTViewModel(targetVM);
        }

        protected override bool UpdateTViewModel(SupplierViewModel targetVM)
        {
            return CUDService.Update(ModifyTargetVM, (ShipOwnerSelectVM != null) ? ShipOwnerSelectVM.SelectedItems : null);
        }

    }
}

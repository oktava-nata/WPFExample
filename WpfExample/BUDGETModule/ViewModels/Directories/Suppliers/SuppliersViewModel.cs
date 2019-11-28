using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using BUDGETModule.VMCollectionLoaders;
using BUDGETModule.VMServices;
using SharedModule.VMCollectionLoaders;
using UIMessager.Services.Message;
using VMBaseSolutions.CollectionVMs;

namespace BUDGETModule.ViewModels.Directories.Suppliers
{
    class SuppliersViewModel : CollectionWithCRUDCommandsVM<SupplierViewModel>
    {
        #region Properties  
        ShipOwnerVMCollectionLoader ShipOwnerLoader
        {
            get
            {
                if (_ShipOwnerLoader == null) _ShipOwnerLoader = new ShipOwnerVMCollectionLoader();
                return _ShipOwnerLoader;
            }
        }
        ShipOwnerVMCollectionLoader _ShipOwnerLoader;
        SupplierVMCollectionLoader _Loader;
        #endregion


        public SuppliersViewModel() : base() { }

        protected override void InitializeCommands()
        {
            base.MenuCommandCRUDVM.InitializeAllCommands(OnAddNew, OnEdit, OnDelete);
        }

        protected async override Task<ICollection<SupplierViewModel>> LoadCollectionAsync()
        {
            _Loader = new SupplierVMCollectionLoader();
            return await _Loader.GetAllAsync();
        }


        async void OnModifyCommandExecute(SupplierViewModel vm)
        {
            await CollectionInitializeAsync(vm);
        }

        async Task<bool> DeleteSupplierAsync()
        {
            SupplierVMService service = new SupplierVMService();
            return await service.DeleteAsync(CurrentElement);
        }

        #region Execute Commands
        protected void OnAddNew()
        {
            ViewCreator.SupplierModifyViewForAdding_Show(ShipOwnerLoader, OnModifyCommandExecute);
        }

        protected void OnEdit()
        {
            ViewCreator.SupplierModifyViewForEditing_Show(CurrentElement, ShipOwnerLoader, OnModifyCommandExecute);
        }

        protected async void OnDelete()
        {
            if (CurrentElement == null) return;
            //if (MsgConfirm.Show(Properties.ResourcesMsg.mConfirmDelSupplier) == MessageBoxResult.No) return;

            var result = await UIMessager.Services.Execeptions.ExceptionНandling.HandleDeleteActionAsync(DeleteSupplierAsync);
            if (result.IsSuccess && result.ReturnValue)
                await CollectionInitializeAsync();
        }

        #endregion
    }
}

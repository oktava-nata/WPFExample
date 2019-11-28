using System;
using System.Threading.Tasks;
using BUDGETModule.ViewModels.Directories.BudgetItems;
using BUDGETModule.ViewModels.Directories.Currencies;
using BUDGETModule.ViewModels.Directories.Suppliers;
using SharedModule.VMCollectionLoaders;

namespace BUDGETModule
{
    public class ViewCreator
    {

        #region Directories
        public async static void CurrencyView_Show()
        {
            var viewModel = new CurrenciesViewModel();
            await viewModel.InitAsync();
            var f = new Views.Directories.CurrencyView();
            f.DataContext = viewModel;
            f.ShowDialog();
        }

        public async static void SuppliersView_Show()
        {
            var viewModel = new SuppliersViewModel();
            await viewModel.InitAsync();
            var f = new Views.Directories.Suppliers.SuppliersView();
            f.DataContext = viewModel;
            f.ShowDialog();

        }

        public async static void SupplierModifyViewForAdding_Show(ShipOwnerVMCollectionLoader shipOwnerLoader, Action<SupplierViewModel> onChanged)
        {
            var viewModel = new VMBaseSolutions.ModifyVMs.ModifyOnWidowVM<SupplierModifyViewModel, SupplierViewModel>(new SupplierModifyViewModel(shipOwnerLoader, onChanged));
            await viewModel.Initialize_ActionAddAsync();
            var f = new Views.Directories.Suppliers.SupplierModifyView();
            f.DataContext = viewModel;
            f.ShowDialog();

        }

        public async static void SupplierModifyViewForEditing_Show(SupplierViewModel source, ShipOwnerVMCollectionLoader shipOwnerLoader, Action<SupplierViewModel> onChanged)
        {
            var viewModel = new VMBaseSolutions.ModifyVMs.ModifyOnWidowVM<SupplierModifyViewModel, SupplierViewModel>(new SupplierModifyViewModel(shipOwnerLoader, onChanged));
            await viewModel.Initialize_ActionEditAsync(source);
            var f = new Views.Directories.Suppliers.SupplierModifyView();
            f.DataContext = viewModel;
            f.ShowDialog();

        }

            #endregion

    }
}

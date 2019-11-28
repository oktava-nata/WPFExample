using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using BUDGETModule.VMCollectionLoaders;
using BUDGETModule.VMServices;
using UIMessager.Services.Message;
using VMBaseSolutions.CollectionVMs;

namespace BUDGETModule.ViewModels.Directories.Currencies
{
    class CurrenciesViewModel : CollectionWithCRUDCommandsAndViewModifyVM<CurrencyViewAndModifyVM, CurrencyViewModel> 
    {

        protected CurrencyVMCollectionLoader _Loader;

        public CurrenciesViewModel() : base() { }

        protected override async Task<ICollection<CurrencyViewModel>> LoadCollectionAsync()
        {
            _Loader = new CurrencyVMCollectionLoader();
            return await _Loader.GetAllCurrencyAsync();
        }

        protected override ICollection<CurrencyViewModel> LoadCollection()
        {
            _Loader = new CurrencyVMCollectionLoader();
            return _Loader.GetAllCurrency();
        }

        async Task<bool> DeleteCurrencyAsync()
        {
            CurrencyVMService service = new CurrencyVMService();
            return await service.DeleteAsync(CurrentElement);
        }

        #region Execute Commands

        protected async override void OnDeleteCommandExecute()
        {
            if (CurrentElement == null) return;

            if (CurrentElement == null) return;
           // if (MsgConfirm.Show(Properties.ResourcesMsg.mConfirmDelCurrency) == MessageBoxResult.No) return;

            var result = await UIMessager.Services.Execeptions.ExceptionНandling.HandleDeleteActionAsync(DeleteCurrencyAsync);
            if (result.IsSuccess && result.ReturnValue)
                await CollectionInitializeAsync();
        }


        #endregion
    }
}

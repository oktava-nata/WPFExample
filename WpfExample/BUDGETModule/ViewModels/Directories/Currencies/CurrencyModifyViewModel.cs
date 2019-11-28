using BUDGETModule.VMFactories;
using BUDGETModule.VMServices;
using Models.Budget;
using VMBaseSolutions.ModifyVMs;

namespace BUDGETModule.ViewModels.Directories.Currencies
{
    class CurrencyViewAndModifyVM : ViewAndModifyWithFactoryAndCUDServiceVM<CurrencyVMFactory, CurrencyVMService, CurrencyViewModel, Currency>
    {
    }

   
}

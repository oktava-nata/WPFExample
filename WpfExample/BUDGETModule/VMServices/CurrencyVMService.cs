using BUDGETModule.ViewModels.Directories.Currencies;
using Infrastructure.Domain;
using Models.Budget;
using VMBaseSolutions.VMServices;

namespace BUDGETModule.VMServices
{
    class CurrencyVMService : CUDVMService<CurrencyViewModel, Currency>
    {
        protected override ICUDService<Currency> CreateCUDService()
        {
            return Domain.Services.Factories.CurrencyServicesFactory.CreateCurrencyCUDSService();
        }



    }
}

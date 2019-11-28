using System.Threading.Tasks;
using BUDGETModule.ViewModels.Directories.Currencies;
using Domain.Common.Services.Budget;
using VMBaseSolutions.VMFactories;

namespace BUDGETModule.VMFactories
{
    class CurrencyVMFactory : IVMFactory<CurrencyViewModel>
    {
        ICurrencyReadService _Service;

        public CurrencyVMFactory()
        {
            _Service = Domain.Services.Factories.CurrencyServicesFactory.CreateCurrencyReadService();
        }


        public async Task<CurrencyViewModel> GetByIdAsync(int itemId)
        {
            var entity = await _Service.GetByIdAsync(itemId);
            return new CurrencyViewModel(entity);
        }
        public CurrencyViewModel GetById(int itemId)
        {
            var entity = _Service.GetById(itemId);
            return new CurrencyViewModel(entity);
        }


        public CurrencyViewModel Create()
        {
            Models.Budget.Currency model = new Models.Budget.Currency();
            return new CurrencyViewModel(model);
        }


    }
}

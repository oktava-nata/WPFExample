using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BUDGETModule.ViewModels.Directories.Currencies;
using Domain.Common.Services.Budget;
using Models.Budget;

namespace BUDGETModule.VMCollectionLoaders
{
    class CurrencyVMCollectionLoader
    {
        ICurrencyReadService _Service;

        public ObservableCollection<CurrencyViewModel> CurrencyVMList { get; private set; }

        static Expression<Func<CurrencyViewModel, string>> _OrderByDefaultExpression { get { return i => i.Code; } }


        public CurrencyVMCollectionLoader()
        {
            _Service = Domain.Services.Factories.CurrencyServicesFactory.CreateCurrencyReadService();
        }

        public async Task<ObservableCollection<CurrencyViewModel>> GetAllCurrencyAsync()
        {
            var listModels = await _Service.GetAllAsync();
            var listVM = VMBaseSolutions.VMEntities.VMEntityListConvertor<CurrencyViewModel, Currency>.ConvertToViewModel(listModels).OrderBy(_OrderByDefaultExpression.Compile());
            CurrencyVMList = new ObservableCollection<CurrencyViewModel>(listVM);
            return CurrencyVMList;
        }

        public ObservableCollection<CurrencyViewModel> GetAllCurrency()
        {
            var listModels = _Service.GetAll();
            var listVM = VMBaseSolutions.VMEntities.VMEntityListConvertor<CurrencyViewModel, Currency>.ConvertToViewModel(listModels).OrderBy(_OrderByDefaultExpression.Compile());
            CurrencyVMList = new ObservableCollection<CurrencyViewModel>(listVM);
            return CurrencyVMList;
        }

        
    }
}

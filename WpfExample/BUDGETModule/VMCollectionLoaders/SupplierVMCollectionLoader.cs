using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using BUDGETModule.ViewModels.Directories.Suppliers;
using Domain.Common.Services.Budget;
using Models.Budget;

namespace BUDGETModule.VMCollectionLoaders
{
    class SupplierVMCollectionLoader
    {
        ISupplierReadService _Service;

        public ObservableCollection<SupplierViewModel> SupplierVMList { get; private set; }

        static Expression<Func<SupplierViewModel, string>> _OrderByDefaultExpression { get { return i => i.Name; } }


        public SupplierVMCollectionLoader()
        {
            _Service = Domain.Services.Factories.SupplierServicesFactory.CreateSupplierReadService();
        }


        public async Task<ObservableCollection<SupplierViewModel>> GetAllAsync()
        {
            var listModels = await _Service.GetAllAsync(i=>i.ShipOwners);
            var listVM = VMBaseSolutions.VMEntities.VMEntityListConvertor<SupplierViewModel, Supplier>.ConvertToViewModel(listModels).OrderBy(_OrderByDefaultExpression.Compile());
            return new ObservableCollection<SupplierViewModel>(listVM);
        }

        public ObservableCollection<SupplierViewModel> GetAll()
        {
            var listModels = _Service.GetAll(i => i.ShipOwners);
            var listVM = VMBaseSolutions.VMEntities.VMEntityListConvertor<SupplierViewModel, Supplier>.ConvertToViewModel(listModels).OrderBy(_OrderByDefaultExpression.Compile());
            return new ObservableCollection<SupplierViewModel>(listVM);
        }
    }
}

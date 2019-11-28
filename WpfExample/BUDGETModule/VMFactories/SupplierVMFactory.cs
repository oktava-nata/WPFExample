using System.Threading.Tasks;
using BUDGETModule.ViewModels.Directories.Suppliers;
using Domain.Common.Services.Budget;
using VMBaseSolutions.VMFactories;

namespace BUDGETModule.VMFactories
{
    class SupplierVMFactory : IVMFactory<SupplierViewModel>
    {
        ISupplierReadService _Service;

        public SupplierVMFactory()
        {
            _Service = Domain.Services.Factories.SupplierServicesFactory.CreateSupplierReadService();
        }


        public async Task<SupplierViewModel> GetByIdAsync(int itemId)
        {
            var entity = await _Service.GetByIdAsync(itemId, i=>i.ShipOwners);
            return new SupplierViewModel(entity);
        }
        public SupplierViewModel GetById(int itemId)
        {
            var entity = _Service.GetById(itemId, i => i.ShipOwners);
            return new SupplierViewModel(entity);
        }

        public SupplierViewModel Create()
        {
            Models.Budget.Supplier model = new Models.Budget.Supplier();
            return new SupplierViewModel(model);
        }
    }
}

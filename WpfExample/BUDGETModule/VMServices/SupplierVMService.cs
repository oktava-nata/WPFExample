using System;
using System.Collections.Generic;
using System.Linq;
using BUDGETModule.ViewModels.Directories.Suppliers;
using Infrastructure.Domain;
using Models.Budget;
using Models.ShipArea;
using SharedModule.ViewModels.Directories;
using VMBaseSolutions.VMServices;

namespace BUDGETModule.VMServices
{
    class SupplierVMService : CUDVMService<SupplierViewModel, Supplier>
    {
        protected override ICUDService<Supplier> CreateCUDService()
        {
            return CreateSupplierCUDService();
        }

        protected Domain.Common.Services.Budget.ISupplierCUDService CreateSupplierCUDService()
        {
            return Domain.Services.Factories.SupplierServicesFactory.CreateSupplierCUDSService();
        }

        public bool Update(SupplierViewModel vm, ICollection<ShipOwnerViewModel> updateShipOwnerList)
        {
            if (vm == null) throw new ArgumentNullException("Arg vm is null!");
            IEnumerable<ShipOwner> shipOwnerForInclude = null;
            IEnumerable<ShipOwner> shipOwnerForExclude = null;
            if (updateShipOwnerList != null)
            {
                List<int> updateShipOwnerIdList = updateShipOwnerList.Select(i => i.Id).ToList();
                List<int> currentShipOwnerIdList = vm.ShipOwnerVMList.Select(i => i.Id).ToList();
                var forAddList = updateShipOwnerIdList.Except(currentShipOwnerIdList).ToList();
                var forDelList = currentShipOwnerIdList.Except(updateShipOwnerIdList).ToList();
                var shipOwnerVMForInclude = updateShipOwnerList.Where(i => forAddList.Contains(i.Id));
                var shipOwnerVMForExclude = vm.ShipOwnerVMList.Where(i => forDelList.Contains(i.Id));
                shipOwnerForInclude = VMBaseSolutions.VMEntities.VMEntityListConvertor<ShipOwnerViewModel, ShipOwner>.ConvertToModel(shipOwnerVMForInclude);
                shipOwnerForExclude = VMBaseSolutions.VMEntities.VMEntityListConvertor<ShipOwnerViewModel, ShipOwner>.ConvertToModel(shipOwnerVMForExclude);
            }
            using (var serv = CreateSupplierCUDService())
            {
                return serv.Update(vm.GetEntity(), shipOwnerForInclude, shipOwnerForExclude);
            }
        }
    }
}

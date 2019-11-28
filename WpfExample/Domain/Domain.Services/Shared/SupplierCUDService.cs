using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common.Services.Budget;
using Domain.EFContexts.Contexts;
using Domain.EFContexts.Factories;
using Infrastructure.EF;
using Models.Budget;
using Models.ShipArea;

namespace Domain.Services.Shared
{

    internal class SupplierCUDService : GenericCUDService<Supplier>, ISupplierCUDService
    {

        internal SupplierCUDService(BudgetContext unitofwork) : base(unitofwork)
        {
        }

        public SupplierCUDService() : base(new BudgetUnitOfWorkFactory().CreateBudgetContext())
        {
        }

        public override bool Add(Supplier entity)
        {
            attachShipOwnerCollection(entity.ShipOwners);
            return base.Add(entity);
        }

        public async override Task<bool> AddAsync(Supplier entity)
        {
            attachShipOwnerCollection(entity.ShipOwners);
            return await base.AddAsync(entity);
        }

        public bool Update(Supplier sourceEntity, IEnumerable<ShipOwner> shipOwnerForInclude, IEnumerable<ShipOwner> shipOwnerForExclude)
        {
            if (shipOwnerForInclude == null && shipOwnerForExclude == null) return base.Update(sourceEntity);

            Attach(sourceEntity);
            attachShipOwnerCollection(shipOwnerForInclude);
            if (shipOwnerForInclude != null)
                foreach (var item in shipOwnerForInclude) sourceEntity.ShipOwners.Add(item);
            if (shipOwnerForExclude != null)
                foreach (var item in shipOwnerForExclude) sourceEntity.ShipOwners.Remove(item);

            Entry(sourceEntity).State = EntityState.Modified;
            return Context.SaveChanges() > 0;
        }

        /// <summary>
        /// Обработка связей с судовладельцами
        /// </summary>
        void attachShipOwnerCollection(IEnumerable<ShipOwner> shipOwnerCollection)
        {
            if (shipOwnerCollection == null || shipOwnerCollection.Count() == 0) return;
            var shipOwnerService = new Domain.Services.ShipArea.ShipOwnerCUDService(this.Context as BudgetContext);
            foreach (var item in shipOwnerCollection.Where(i => i.Id > 0))
                shipOwnerService.Attach(item);
        }
    }
}

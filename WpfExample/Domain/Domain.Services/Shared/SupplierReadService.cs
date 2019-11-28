using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Common.Services.Budget;
using Domain.EFContexts.Contexts;
using Domain.EFContexts.Factories;
using Domain.Services.Specifications.Budget;
using Infrastructure.EF;
using Models.Budget;

namespace Domain.Services.Shared
{

    internal class SupplierReadService : GenericReadService<Supplier, BudgetContext>, ISupplierReadService
    {
        protected override BudgetContext CreateContext()
        {
            return new BudgetUnitOfWorkFactory().CreateBudgetContext();
        }

        public SupplierReadService() : base()
        {
        }

        public async Task<ICollection<Supplier>> GetAllAsync(params Expression<Func<Supplier, object>>[] includeProperties)
        {
            return await base.GetCollectionAsync(includeProperties: includeProperties);
        }

        public ICollection<Supplier> GetAll(params Expression<Func<Supplier, object>>[] includeProperties)
        {
            return base.GetCollection(includeProperties: includeProperties);
        }


        public async Task<ICollection<Supplier>> GetAllByShipownerAsync(int shipownerId)
        {
            return await base.GetCollectionAsync(new SuppliersByShipownerSpecification(shipownerId));
        }

        public ICollection<Supplier> GetAllByShipowner(int shipownerId)
        {
            return base.GetCollection(new SuppliersByShipownerSpecification(shipownerId));
        }


        public async Task<ICollection<Supplier>> GetAllUsedInCompanyBudgetAsync()
        {
            return await base.GetCollectionAsync(new SuppliersUsedInCompanyBudgetSpecification());
        }

        public ICollection<Supplier> GetAllUsedInCompanyBudget()
        {
            return base.GetCollection(new SuppliersUsedInCompanyBudgetSpecification());
        }


    }
}

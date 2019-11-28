using System.Linq;
using Models.Budget;

namespace Domain.Services.Specifications.Budget
{
    class SuppliersByShipownerSpecification : Infrastructure.EF.Specification<Supplier>
    {
        public SuppliersByShipownerSpecification(int shipownerId) 
        {
            this.Predicate = i => i.ShipOwners.Any(j=>j.Id == shipownerId);
        }
    }

    class SuppliersUsedInCompanyBudgetSpecification : Infrastructure.EF.Specification<Supplier>
    {
        public SuppliersUsedInCompanyBudgetSpecification()
        {
            this.Predicate = i => i.IsUsedInCompanyBudget;
        }
    }
}

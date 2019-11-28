using Models.Budget;

namespace Domain.Services.Specifications.Budget
{
    class BudgetItemsByShipownerSpecification : Infrastructure.EF.Specification<BudgetItem>
    {
        public BudgetItemsByShipownerSpecification(int? shipownerId) 
        {
            this.Predicate = i => i.IdShipOwner == shipownerId;
        }
    }

    class BudgetItemsByParentSpecification : Infrastructure.EF.Specification<BudgetItem>
    {
        public BudgetItemsByParentSpecification(int? idParentItem)
        {
            this.Predicate = i => i.IdParent == idParentItem;
        }
    }
}

namespace Domain.EFContexts.Factories
{
    public class BudgetUnitOfWorkFactory
    {
        public Contexts.BudgetContext CreateBudgetContext()
        {
            return new Contexts.BudgetContext(InfrastructureEFInitializer.Configuration);
        }
    }
}

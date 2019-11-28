namespace Domain.EFContexts.Factories
{
    public class ShipAreaUnitOfWorkFactory
    {
        public Contexts.ShipAreaContext CreateShipAreaContext()
        {
            return new Contexts.ShipAreaContext(InfrastructureEFInitializer.Configuration);
        }
    }
}

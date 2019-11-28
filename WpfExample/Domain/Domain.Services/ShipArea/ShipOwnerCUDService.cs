using Domain.Common.Services.Budget;
using Domain.Common.Services.ShipArea;
using Domain.EFContexts.Contexts;
using Domain.EFContexts.Factories;
using Infrastructure.Domain;
using Infrastructure.EF;
using Models.Budget;
using Models.ShipArea;

namespace Domain.Services.ShipArea
{

    internal class ShipOwnerCUDService : GenericCUDService<ShipOwner>, IShipOwnerCUDService
    {
        internal ShipOwnerCUDService(ShipAreaContext unitofwork) : base(unitofwork) { }
        internal ShipOwnerCUDService(BudgetContext unitofwork) : base(unitofwork) { }

        public ShipOwnerCUDService() : base(new ShipAreaUnitOfWorkFactory().CreateShipAreaContext()) { }

    }
}

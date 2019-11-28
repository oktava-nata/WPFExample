using Domain.Common.Services.ShipArea;
using Domain.Services.ShipArea;

namespace Domain.Services.Factories
{
    public static class ShipOwnerServicesFactory
    {

        public static IShipOwnerReadServices CreateShipOwnerReadService()
        {
            return new ShipOwnerReadService();
        }
    }
}

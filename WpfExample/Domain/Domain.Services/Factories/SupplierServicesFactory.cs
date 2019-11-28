using Domain.Common.Services.Budget;
using Domain.Services.Shared;

namespace Domain.Services.Factories
{
    public static class SupplierServicesFactory
    {

        public static ISupplierReadService CreateSupplierReadService()
        {
            return new SupplierReadService();
        }

        public static ISupplierCUDService CreateSupplierCUDSService()
        {
            return new SupplierCUDService();
        }
    }
}

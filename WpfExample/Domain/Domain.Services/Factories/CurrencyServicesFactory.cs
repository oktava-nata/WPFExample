using Domain.Common.Services.Budget;
using Domain.Services.Budget;

namespace Domain.Services.Factories
{
    public static class CurrencyServicesFactory
    {

        public static ICurrencyReadService CreateCurrencyReadService()
        {
            return new CurrencyReadService();
        }

        public static ICurrencyCUDService CreateCurrencyCUDSService()
        {
            return new CurrencyCUDService();
        }
    }
}

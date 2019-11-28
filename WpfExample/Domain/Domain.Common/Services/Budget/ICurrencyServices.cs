using Infrastructure.Domain;
using Models.Budget;

namespace Domain.Common.Services.Budget
{
    public interface ICurrencyReadService : IHasGetByIdMethods<Currency>, IHasGetAllMethods<Currency>
    {
    }

    public interface ICurrencyCUDService : ICUDService<Currency>
    {
    }

}

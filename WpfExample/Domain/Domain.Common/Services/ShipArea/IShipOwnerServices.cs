using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Domain;

namespace Domain.Common.Services.ShipArea
{
    public interface IShipOwnerReadServices : IHasGetByIdMethods<Models.ShipArea.ShipOwner>, IHasGetAllMethods<Models.ShipArea.ShipOwner>
    {
        Task<ICollection<Models.ShipArea.ShipOwner>> GetAllWithSameIdAsync(List<int> ids);
        ICollection<Models.ShipArea.ShipOwner> GetAllWithSameId(List<int> ids);
    }

    public interface IShipOwnerCUDService : ICUDService<Models.ShipArea.ShipOwner>
    {
    }

}

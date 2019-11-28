using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common.Services.ShipArea;
using Domain.EFContexts.Contexts;
using Domain.EFContexts.Factories;
using Infrastructure.EF;
using Models.ShipArea;

namespace Domain.Services.ShipArea
{

    internal class ShipOwnerReadService : GenericReadService<Models.ShipArea.ShipOwner, ShipAreaContext>, IShipOwnerReadServices
    {
        protected override ShipAreaContext CreateContext()
        {
            return new ShipAreaUnitOfWorkFactory().CreateShipAreaContext();
        }

        public ShipOwnerReadService() : base()
        {
        }

        public async Task<ICollection<Models.ShipArea.ShipOwner>> GetAllAsync()
        {
            return await base.GetCollectionAsync();
        }

        public ICollection<Models.ShipArea.ShipOwner> GetAll()
        {
            return base.GetCollection();
        }

        public async Task<ICollection<ShipOwner>> GetAllWithSameIdAsync(List<int> ids)
        {
            GenericService<ShipOwner> service;
            var set = base.GetForRead(out service);
            var result = await set.Join(ids, s => s.Id, i => i, (s, i) => s).ToListAsync();
            service.Dispose();
            return result;

        }

        public ICollection<ShipOwner> GetAllWithSameId(List<int> ids)
        {
            GenericService<ShipOwner> service;
            var set = base.GetForRead(out service);
            var result = set.Join(ids, s => s.Id, i => i, (s, i) => s).ToList();
            service.Dispose();
            return result;
        }
    }
}

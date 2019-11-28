using System.Data.Entity;
using Infrastructure.Domain;
using Infrastructure.EF;
using Models.ShipArea;

namespace Domain.EFContexts.Contexts
{
    public class ShipAreaContext: BaseContext
    {
        internal ShipAreaContext(IEFInitializer configuration) : base(configuration) { }
     
        public virtual DbSet<ShipOwner> ShipOwners { get; set; }


    }
}

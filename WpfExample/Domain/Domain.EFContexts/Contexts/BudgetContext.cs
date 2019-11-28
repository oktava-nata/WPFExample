using System.Data.Entity;
using Infrastructure.Domain;
using Infrastructure.EF;
using Models.Budget;
using Models.ShipArea;

namespace Domain.EFContexts.Contexts
{
    public class BudgetContext : BaseContext
    {
        internal BudgetContext(IEFInitializer configuration) : base(configuration) { }

        public virtual DbSet<BudgetItem> BudgetItems { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<ShipOwner> ShipOwners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Supplier>()
                       .HasMany(s => s.ShipOwners)
                       .WithMany(s => s.Suppliers)
                       .Map(cs =>
                       {
                           cs.MapLeftKey("IdSupplier");
                           cs.MapRightKey("IdShipOwner");
                           cs.ToTable("bdg_Supplier_ShipOwner");
                       });
        }

    }
}

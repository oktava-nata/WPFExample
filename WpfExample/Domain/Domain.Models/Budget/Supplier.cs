using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Domain;
using Models.ShipArea;

namespace Models.Budget
{
    [Table("bdg_Supplier")]
    public class Supplier : IAggregateRootEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]              
        public string Name { get; set; }

        public string Address{ get; set; }

        public string ContactPerson { get; set; }

        public string Phone { get; set; }       

        public bool IsUnscrupulousSupplier { get; set; }

        public bool IsUsedInCompanyBudget { get; set; }

        public virtual ICollection<ShipOwner> ShipOwners { get; private set; }

        [NotMapped]
        public int FilesCount { get; protected set; }

        [NotMapped]
        public bool HasFiles;

        public Supplier()
        {
            ShipOwners = new HashSet<ShipOwner>();
        }

        public Supplier(string name) :this()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Supplier name is null or empty!");                      
            Name = name;          
        }

        public void UpdateShipOwners(IEnumerable<ShipOwner> shipOwnerList)
        {
            ShipOwners = new HashSet<ShipOwner>(shipOwnerList);
        }

        class EntityComparer<Entity> : IEqualityComparer<Entity> where Entity : IEntity
        {
            public bool Equals(Entity x, Entity y)
            {
                if (x == null && y != null || x != null && y == null) return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(Entity obj)
            {
                return obj.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IAggregateRootEntity)) return false;
            return this.Id== (obj as IAggregateRootEntity).Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

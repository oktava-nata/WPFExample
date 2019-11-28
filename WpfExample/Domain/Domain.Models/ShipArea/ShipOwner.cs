using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Domain;
using Models.Budget;

namespace Models.ShipArea
{
    [Table("_ShipOwner")]
    public class ShipOwner : IAggregateRootEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
       
        public string Name { get; set; }

        public virtual ICollection<Supplier> Suppliers { get; private set; }
        public virtual ICollection<Ship> Ships { get; private set; }

        public ShipOwner()
        {
            Ships = new HashSet<Ship>(); 
            Suppliers = new HashSet<Supplier>();
        }

        public ShipOwner(string name) :this()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("BudgetItem name is null or empty!");
            Name = name;
        }

       
    }
}

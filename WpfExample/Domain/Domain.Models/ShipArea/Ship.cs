using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Domain;

namespace Models.ShipArea
{
    [Table("_Ship")]
    public class Ship : IAggregateRootEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdShip { get; set; }

        [NotMapped]
        public int Id
        {
            get { return IdShip; }
            set { IdShip = value; }
        }

        [Required]
        public string Number { get; set; }

        [Required]       
        public string Name { get; set; }

        public Ship()
        {          
        }

        public Ship(string name) :this()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Vessel name is null or empty!");
            Name = name;
        }

       
    }
}

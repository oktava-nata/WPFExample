using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Domain;

namespace Models.Budget
{
    [Table("bdg_Currency")]
    public class Currency : IAggregateRootEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]              
        public string Code { get; set; }
        [Required]
        public string Name{ get; set; }

        public bool VAT { get; set; }          

        public Currency()
        {             
        }
      
        public Currency(string code, string name, bool vat) :this()
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException("Currency code is null or empty!");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Currency name is null or empty!");
            Code = code;
            Name = name;
            VAT = vat;
        }
       

    }
}

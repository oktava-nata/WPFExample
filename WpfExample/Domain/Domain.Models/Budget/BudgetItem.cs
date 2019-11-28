using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Domain;
using Models.ShipArea;

namespace Models.Budget
{
    [Table("bdg_BudgetItem")]
    public class BudgetItem: IAggregateRootEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        
        public string Name_RU { get; set; }

        public string Name_EN { get; set; }

        public string Description { get; set; }

        public int? IdParent { get; private set; }

        public int? IdShipOwner { get; private set; }

        public bool IsInBudgetPlanByDefault { get; set; }

        public bool IsGroupingItem { get; set; }

        [ForeignKey("IdParent")]
        public virtual BudgetItem ParentItem { get; private set; }
        [ForeignKey("IdShipOwner")]
        public virtual ShipOwner ShipOwner { get; private set; }
        public virtual ICollection<BudgetItem> ChildrenItems { get; private set; }

        //protected
        public BudgetItem()
        {
            ChildrenItems = new HashSet<BudgetItem>(); 
        }

        public BudgetItem(string code, string name_ru, string name_en) :this()
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException("BudgetItem code is null or empty!");
            if (string.IsNullOrWhiteSpace(name_ru) && string.IsNullOrWhiteSpace(name_en))
                throw new ArgumentNullException("BudgetItem name is null or empty on both languages!");
            Code = code;
            Name_RU = name_ru;
            Name_EN = name_en;
        }

        public void SetParentItem(BudgetItem parentItem)
        {
            IdParent = (parentItem!=null)? (int?)parentItem.Id : null;
            ParentItem = parentItem;
        }

        public void SetParentItem(int? parentItemId)
        {
            IdParent = parentItemId;
        }

        public void SetShipOwner(ShipOwner shipOwner)
        {
            if (shipOwner == null) throw new ArgumentNullException("Ship Owner is null!");
            SetShipOwner(shipOwner.Id);
        }

        public void SetShipOwner(int shipOwnerId)
        {
            IdShipOwner = shipOwnerId;
        }

        public void InsertChildrenItem(BudgetItem parentItem)
        {
            ChildrenItems.Add(parentItem);
        }

    }
}

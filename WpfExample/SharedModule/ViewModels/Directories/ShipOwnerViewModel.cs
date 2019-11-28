using System;
using System.Threading.Tasks;
using Models.Budget;
using Models.ShipArea;
using Telerik.Windows.Data;
using VMBaseSolutions.VMEntities;

namespace SharedModule.ViewModels.Directories
{
    public class ShipOwnerViewModel : VMEntityBase<ShipOwner>
    {
        #region Properties
        public override int Id => _entity.Id;

        public string Name
        {
            get { return _entity.Name; }
            set
            {
                _entity.Name = value; OnPropertyChanged(() => this.Name);
            }
        }

       
        #endregion

        #region Constructors and PrivateMethods
        public ShipOwnerViewModel() : base(new ShipOwner()) { }
        public ShipOwnerViewModel(ShipOwner entity) : base(entity) { }

        #endregion



        public override string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name": return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Name);
                }
                return null;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
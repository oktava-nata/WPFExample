using System.Collections.Generic;
using System.Collections.ObjectModel;
using Models.Budget;
using Models.ShipArea;
using SharedModule.ViewModels.Directories;
using Telerik.Windows.Data;
using VMBaseSolutions.VMEntities;
using System.Linq;

namespace BUDGETModule.ViewModels.Directories.Suppliers
{
    public class SupplierViewModel : VMEntityBase<Supplier>
    {
        #region Properties
        public override int Id => _entity.Id;

        public string Name
        {
            get { return _entity.Name; }
            set { _entity.Name = value; OnPropertyChanged(() => this.Name); }
        }

        public string Address
        {
            get { return _entity.Address; }
            set
            {
                _entity.Address = value; OnPropertyChanged(() => this.Address);
            }
        }

        public string ContactPerson
        {
            get { return _entity.ContactPerson; }
            set
            {
                _entity.ContactPerson = value; OnPropertyChanged(() => this.ContactPerson);
            }
        }

        public string Phone
        {
            get { return _entity.Phone; }
            set { _entity.Phone = value; OnPropertyChanged(() => this.Phone); }
        }

        public int FilesCount
        {
            // get { return _entity.FilesCount; }
            get { return 2; }
        }

        public bool HasFiles
        {
            // get { return _entity.HasFiles; }
            get { return true; }
        }

        public bool IsUsedInCompanyBudget
        {
            get { return _entity.IsUsedInCompanyBudget; }
            set { _entity.IsUsedInCompanyBudget = value; OnPropertyChanged(() => this.IsUsedInCompanyBudget); }
        }

        public bool IsUnscrupulousSupplier
        {
            get { return _entity.IsUnscrupulousSupplier; }
            set { _entity.IsUnscrupulousSupplier = value; OnPropertyChanged(() => this.IsUnscrupulousSupplier); }
        }

        public ObservableCollection<ShipOwnerViewModel> ShipOwnerVMList
        {
            get { return _ShipOwnerVMList; }
            private set { _ShipOwnerVMList = value; OnPropertyChanged(() => this.ShipOwnerVMList); }
        }
        ObservableCollection<ShipOwnerViewModel> _ShipOwnerVMList;

        public string ShipOwnerListAsString
        {
            get
            {
                if (_ShipOwnerListAsString == null) generate_ShipOwnerListAsString();
                return _ShipOwnerListAsString;
            }
        }
        string _ShipOwnerListAsString;

        #endregion

        #region Constructors and PrivateMethods
        public SupplierViewModel() : base(new Supplier()) { }
        public SupplierViewModel(Supplier entity) : base(entity) { }

        public override void SetEntity(Supplier entity)
        {
            base.SetEntity(entity);
            var list = VMBaseSolutions.VMEntities.VMEntityListConvertor<ShipOwnerViewModel, ShipOwner>.ConvertToViewModel(entity.ShipOwners);
            _ShipOwnerVMList = new ObservableCollection<ShipOwnerViewModel>(list);
        }
        #endregion

        public void UpdateShipOwnerList(IEnumerable<ShipOwnerViewModel> shipOwnerList)
        {
            _entity.UpdateShipOwners(VMBaseSolutions.VMEntities.VMEntityListConvertor<ShipOwnerViewModel, ShipOwner>.ConvertToModel(shipOwnerList));
            var list = VMBaseSolutions.VMEntities.VMEntityListConvertor<ShipOwnerViewModel, ShipOwner>.ConvertToViewModel(_entity.ShipOwners);
            ShipOwnerVMList = new ObservableCollection<ShipOwnerViewModel>(list);
        }

        void generate_ShipOwnerListAsString()
        {
            if (ShipOwnerVMList != null && ShipOwnerVMList.Count > 0)
            {
                var list = ShipOwnerVMList.OrderBy(SharedModule.VMCollectionLoaders.ShipOwnerVMCollectionLoader._OrderByDefaultExpression.Compile()).Select(i => i.ToString()).ToList();
                _ShipOwnerListAsString = string.Join(", ", list);
            }
            else _ShipOwnerListAsString = null;
        }


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
using DBServices.MRV;
using DBServices.UsualEntity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ViewModelBaseSolutions.UIEntityHelper;

namespace Shared.UI.Vessel
{
    public class UI_Ship : UI_CEntity<Ship>
    {
        #region Properties
        #region Simple Properties              

        public string Name
        {
            get { return _entity.Name; }
            set { _entity.Name = value; RaisePropertyChanged(() => this.Name); }
        }

        public string Number
        {
            get { return _entity.Number; }
            set { _entity.Number = value; RaisePropertyChanged(() => this.Number); }
        }

        public DateTime? IssueDate
        {
            get { return _entity.IssueDate; }
            set { _entity.IssueDate = value; RaisePropertyChanged(() => this.IssueDate); }
        }

        //public int? IdShipType
        //{
        //    get { return _entity.IdShipType; }
        //    set { _entity.IdShipType = value; RaisePropertyChanged(() => this.IdShipType); }
        //}

        public int? IdShipGroup
        {
            get { return _entity.IdShipGroup; }
            set { _entity.IdShipGroup = value; RaisePropertyChanged(() => this.IdShipGroup); }
        }

        public int SortNumber
        {
            get { return _entity.SortNumber; }
            set { _entity.SortNumber = value; RaisePropertyChanged(() => this.SortNumber); }
        }

        public bool IsHidden
        {
            get { return _entity.IsHidden; }
            set { _entity.IsHidden = value; RaisePropertyChanged(() => this.IsHidden); }
        }
        #endregion

        public UI_ShipType Type
        {
            get
            {               
                return Setting != null ? Setting.Type : null;
            }
            set
            {
                if (Setting != null)
                {
                    Setting.Type = value;
                    RaisePropertyChanged(() => this.Type);
                }
            }
        }        

        public UI_ShipGroup Group
        {
            get
            {
                if (_Group == null && IdShipGroup.HasValue)
                {
                    if (_entity.Group != null)
                        _Group = new UI_ShipGroup(_entity.Group);
                    else
                    {
                        var e = new ShipGroup { Id = IdShipGroup.Value };
                        _Group = new UI_ShipGroup(e);
                    }
                }
                return _Group;
            }
            set
            {
                _Group = value;
                this.IdShipGroup = (value != null) ? (int?)value.Id : null; 
                RaisePropertyChanged(() => this.Group);
            }
        }
        UI_ShipGroup _Group;
      
        public UI_ShipSetting Setting
        {
            get
            {
                if (_Setting == null && _entity.Setting != null)
                    _Setting = new UI_ShipSetting(_entity.Setting);
                return _Setting;
            }
            set { _Setting = value; RaisePropertyChanged(() => this.Setting); }
        }
        UI_ShipSetting _Setting;


        public ObservableCollection<ShipPropertyValue> PropertyValues
        {
            get
            {
                return _entity.PropertyValues;
            }
            set { _entity.PropertyValues = value; RaisePropertyChanged(() => this.PropertyValues); }
        }

        public int? ID { get { return (IsEmpty) ? null : (int?)Id; } }

        public bool IsEmpty { get { return !(Id > 0); } }

        #endregion

        #region Constructors and PrivateMethods
        public UI_Ship() { }
        public UI_Ship(int id) : this(new Ship(id)) { }
        public UI_Ship(Ship entity) : base(entity) { }
    
     

        #endregion

        #region Methods           

        public bool SaveComplex()
        {
            return _entity.SaveComplex();
        }
        #endregion

        public override string ToString()
        {
            return this.Name;
        }

        public override string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name": return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Name);
                    case "Number": return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Number);
                    case "SortNumber": return MVVMHelper.Validators.ValidatorVM.Int_CheckOnInRange(SortNumber, 0);
                }
                return null;              
            }
        }
    }
}

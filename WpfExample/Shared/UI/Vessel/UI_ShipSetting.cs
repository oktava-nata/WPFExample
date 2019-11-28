using ViewModelBaseSolutions.UIEntityHelper;
using DBServices.UsualEntity;
using Shared.UIViewRules;
using Shared.UI.Countries;
using DBServices.PERSONAL;

namespace Shared.UI.Vessel
{
    public class UI_ShipSetting : UI_CEntity<ShipSetting>
    {
        #region Properties
        public int? IdShip
        {
            get { return _entity.IdShip; }
            set { _entity.IdShip = value; this.RaisePropertyChanged(() => this.IdShip); }
        }

        public int? IdShipType
        {
            get { return _entity.IdShipType; }
            set { _entity.IdShipType = value; RaisePropertyChanged(() => this.IdShipType); }
        }

        public string Prefix
        {
            get { return _entity.Prefix; }
            set { _entity.Prefix = value; this.RaisePropertyChanged(() => this.Prefix); }
        }
       
        public DBServices.UnitOfCargoValue UnitOfCargo
        {
            get { return _entity.UnitOfCargo; }
            set { _entity.UnitOfCargo = value; RaisePropertyChanged(() => this.UnitOfCargo); }
        }

        public string CallSign
        {
            get { return _entity.CallSign; }
            set { _entity.CallSign = value; this.RaisePropertyChanged(() => this.CallSign); }
        }


        public int? IdCountry
        {
            get { return _entity.IdCountry; }
            set { _entity.IdCountry = value; this.RaisePropertyChanged(() => this.IdCountry); }
        }

        //Ksanti
        // вспомогательный двигатель
        public decimal? AuxEngine
        {
            get { return _entity.AuxEngine; }
            set { _entity.AuxEngine = value; RaisePropertyChanged(() => this.AuxEngine); }
        }


        //мощность главных двигателей
        public decimal? PowerMainEngine
        {
            get { return _entity.PowerMainEngine; }
            set { _entity.PowerMainEngine = value; RaisePropertyChanged(() => this.PowerMainEngine); }
        }

        // чистая вместимость
        public decimal? NetCapacity
        {
            get { return _entity.NetCapacity; }
            set { _entity.NetCapacity = value; RaisePropertyChanged(() => this.NetCapacity); }
        }

        // валовая вместимость
        public decimal? GrossTonnage
        {
            get { return _entity.GrossTonnage; }
            set { _entity.GrossTonnage = value; RaisePropertyChanged(() => this.GrossTonnage); }
        }

        // DWT
        public decimal? DWT
        {
            get { return _entity.DWT; }
            set { _entity.DWT = value; RaisePropertyChanged(() => this.DWT); }
        }

        // EIV
        public decimal? EIV
        {
            get { return _entity.EIV; }
            set { _entity.EIV = value; RaisePropertyChanged(() => this.EIV); }
        }

        // EEDI
        public decimal? EEDI
        {
            get { return _entity.EEDI; }
            set { _entity.EEDI = value; RaisePropertyChanged(() => this.EEDI); }
        }

        // название судовладельца
        public string ShipownerName
        {
            get { return _entity.ShipownerName; }
            set { _entity.ShipownerName = value; RaisePropertyChanged(() => this.ShipownerName); }
        }

        // адрес судовладельца
        public string ShipownerAddress
        {
            get { return _entity.ShipownerAddress; }
            set { _entity.ShipownerAddress = value; RaisePropertyChanged(() => this.ShipownerAddress); }
        }

        //название компании
        public string CompanyName
        {
            get { return _entity.CompanyName; }
            set { _entity.CompanyName = value; RaisePropertyChanged(() => this.CompanyName); }
        }

        //адрес компании
        public string CompanyAddress
        {
            get { return _entity.CompanyAddress; }
            set { _entity.CompanyAddress = value; RaisePropertyChanged(() => this.CompanyAddress); }
        }

        //ФИО, должность контактного лица
        public string ContactName
        {
            get { return _entity.ContactName; }
            set { _entity.ContactName = value; RaisePropertyChanged(() => this.ContactName); }
        }

        //Номер телефона контактного лица
        public string ContactPhone
        {
            get { return _entity.ContactPhone; }
            set { _entity.ContactPhone = value; RaisePropertyChanged(() => this.ContactPhone); }
        }

        //Адрес контактного лица
        public string ContactAddress
        {
            get { return _entity.ContactAddress; }
            set { _entity.ContactAddress = value; RaisePropertyChanged(() => this.ContactAddress); }
        }

        //E-mail контактного лица
        public string ContactEmail
        {
            get { return _entity.ContactEmail; }
            set { _entity.ContactEmail = value; RaisePropertyChanged(() => this.ContactEmail); }
        }

        //Метод контроля для отчета по энергоэффективности     
        public DBServices.ControlMethodValue? ControlEFMethod
        {
            get { return _entity.ControlEFMethod; }
            set { _entity.ControlEFMethod = value; RaisePropertyChanged(() => this.ControlEFMethod); }
        }

        // Ледовый класс судна
        public string IceClass
        {
            get { return _entity.IceClass; }
            set { _entity.IceClass = value; RaisePropertyChanged(() => this.IceClass); }
        }


        //Показатель технической эффективности судна
        public string TechPerformanceIndicator
        {
            get { return _entity.TechPerformanceIndicator; }
            set { _entity.TechPerformanceIndicator = value; RaisePropertyChanged(() => this.TechPerformanceIndicator); }
        }

        //Порт регистрации
        public string PortRegistry
        {
            get { return _entity.PortRegistry; }
            set { _entity.PortRegistry = value; RaisePropertyChanged(() => this.PortRegistry); }
        }

        public UI_ShipType Type
        {
            get
            {
                if (_Type == null && IdShipType.HasValue)
                {
                    if (_entity.Type != null)
                        _Type = new UI_ShipType(_entity.Type);
                    else
                    {
                        var e = new ShipType { Id = IdShipType.Value };
                        _Type = new UI_ShipType(e);
                    }
                }
                return _Type;
            }
            set
            {
                _Type = value;
                this.IdShipType = (value != null) ? (int?) value.Id : null;
                RaisePropertyChanged(() => this.Type);
            }
        }
        UI_ShipType _Type;


        public UI_Country Country
        {
            get
            {
                if (_Country == null && IdCountry.HasValue)
                {
                    if (_entity.Country != null)
                        _Country = new UI_Country(_entity.Country);
                    else
                    {
                        var e = new Country { Id = IdCountry.Value };
                        _Country = new UI_Country(e);
                    }
                }
                return _Country;
            }
            set
            {
                _Country = value;
                this.IdCountry = (value != null) ? (int?)value.Id : null;
                RaisePropertyChanged(() => this.Country);
            }
        }
        UI_Country _Country;

        #endregion

        #region Constructors and PrivateMethods
        public UI_ShipSetting() { }
        public UI_ShipSetting(int id): this(new ShipSetting(id)) { }
        public UI_ShipSetting(ShipSetting entity) : base(entity) { }

        #endregion

            
        // по идее надо привести к IMO номеру судна
        public override string ToString()
        {
            return (this.IdShip.HasValue) ? string.Empty: this.IdShip.Value.ToString();
        }

        public override string this[string columnName]
        {
            get
            {               
                return null;
            }
        }

    }



}

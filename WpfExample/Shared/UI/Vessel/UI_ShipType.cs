using ViewModelBaseSolutions.UIEntityHelper;
using DBServices.UsualEntity;
using UI;

namespace Shared.UI.Vessel
{
    public class UI_ShipType : UI_CEntity<ShipType>, UI_INameENRU
    {
        #region Properties      

        public string Name_RU
        {
            get { return _entity.Name_RU; }
            set {
                _entity.Name_RU = value;
                RaisePropertyChanged(() => this.Name_RU);
                RaisePropertyChanged(() => this.Name_EN);
            }
        }

        public string Name_EN
        {
            get { return _entity.Name_EN; }
            set {
                _entity.Name_EN = value;
                RaisePropertyChanged(() => this.Name_EN);
                RaisePropertyChanged(() => this.Name_RU);
            }
        }

        //сделать зависимой от языка интерфейса
        public string NameForView
        {
            get { return global::UI.UI_NameENRUService.GetDisplayName(this); }
        }

        public string NameForViewReport
        {
            get { return global::UI.UI_NameENRUService.GetDisplayNameBothLanguages(this, " \n"); }
        }

        public string NameForViewOldReport
        {
            get { return global::UI.UI_NameENRUService.GetDisplayNameBothLanguages(this, "/"); }
        }

        #endregion

        #region Constructors and PrivateMethods
        public UI_ShipType() { }
        public UI_ShipType(int id) : this(new ShipType(id)) { }
        public UI_ShipType(ShipType entity) : base(entity) { }

        #endregion

        
        /// <summary>
        /// Проверка есть ли суда данного типа
        /// </summary>
        public bool HasShips()
        {
            return _entity.HasShips();
        }
       

        public override string ToString()
        {
            return this.NameForView;
        }

        public override string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name_RU":
                        {
                            string name_RU_valid = MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Name_RU);
                            if (name_RU_valid == null)
                                return null;
                            else
                                return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmptyRUAndEN(Name_RU, Name_EN);
                        }
                    case "Name_EN":
                        {
                            string name_EN_valid = MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Name_EN);
                            if (name_EN_valid == null)
                                return null;
                            else
                                return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmptyRUAndEN(Name_RU, Name_EN);
                        }
                }
                return null;
            }
        }

    }



}

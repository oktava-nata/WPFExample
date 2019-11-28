using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelBaseSolutions.UIEntityHelper;
using DBServices.PERSONAL;
using UI;

namespace Shared.UI.Countries
{
 
    public class UI_Country : UI_ChangingEntity<DBServices.PERSONAL.Country>, UI_INameENRU
    {
        #region Properties
        public string Name_RU
        {
                      
            get { return _entity.Name_RU; }
            set { _entity.Name_RU = value; RaisePropertyChanged(() => this.Name_RU); RaisePropertyChanged(() => this.Name_EN); }

        }

        public string Name_EN
        {
         
            get { return _entity.Name_EN;}
            set { _entity.Name_EN = value; RaisePropertyChanged(() => this.Name_EN); RaisePropertyChanged(() => this.Name_RU); }            
        }

        public string NameForView
        {
            get { return global::UI.UI_NameENRUService.GetDisplayName(this); }
        }

        public string NameForViewReport
        {
            get { return global::UI.UI_NameENRUService.GetDisplayNameBothLanguages(this, " \n"); }
        }


        public DBServices.IChangeInfo ChangeInfo
        {
            get { return _entity.ChangeInfoForViewing; }
        }
        #endregion

        #region Constructors and PrivateMethods
        public UI_Country() { }
        public UI_Country(int id) : this(new DBServices.PERSONAL.Country(id)) { }

        public UI_Country(DBServices.PERSONAL.Country entity) : base(entity) { }

        #endregion

        public void LoadChangeInfo()
        {
            this._entity.LoadChangeInfoForView(true, true);
            RaisePropertyChanged(() => this.ChangeInfo);
        }

        public override void Delete()
        {
            _entity.DeleteAndCreateRemoveCommandsForAllShips();
        }

        public override string ToString()
        {
            return UI_NameENRUService.GetDisplayName(this);
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
                                return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Name_EN);
                        }
                    case "Name_EN":
                        {
                            string name_EN_valid = MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Name_EN);
                            if (name_EN_valid == null)
                                return null;
                            else
                                return MVVMHelper.Validators.ValidatorVM.String_CheckOnEmpty(Name_RU);
                        }
                }
                return null;
            }
        }



    }
}

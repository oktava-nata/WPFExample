using DBServices.MRV;
using DBServices.PMS;
using DBServices.ShipManager.PMS.CompanySetting;
using DBServices.UsualEntity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ViewModelBaseSolutions.UIEntityHelper;
using ViewModelBaseSolutions.UIEntityHelper.Files;

namespace Shared.UI.CompanySettings
{
    public class UI_CompanySetting : UI_ChangingEntity<PMS_CompanySetting>
    {
        #region Properties
        public string Prefix
        {
            get { return _entity.Prefix; }
            set { _entity.Prefix = value; RaisePropertyChanged(() => this.Prefix); }
        }

        public bool IsHideFullPath
        {
            get { return _entity.IsHideFullPath; }
            set { _entity.IsHideFullPath = value; RaisePropertyChanged(() => this.IsHideFullPath); }
        }

        public bool DisallowMinNumberOnShip
        {
            get { return _entity.DisallowMinNumberOnShip; }
            set { _entity.DisallowMinNumberOnShip = value; RaisePropertyChanged(() => this.DisallowMinNumberOnShip); }
        }

        public string AdditionalInfoForCrewListMAP
        {
            get { return _entity.AdditionalInfoForCrewListMAP; }
            set { _entity.AdditionalInfoForCrewListMAP = value; RaisePropertyChanged(() => this.AdditionalInfoForCrewListMAP); }
        }

        public UI_File<PMS_CompanySettingLogoFileInfo> Logo
        {
            get { return _Logo; }
            set { _Logo = value; RaisePropertyChanged(() => this.Logo); }
        }
        UI_File<PMS_CompanySettingLogoFileInfo> _Logo;

        public bool UseLastExportDate
        {
            get
            {
                return (_entity.InnerCompanySetting != null) ? _entity.InnerCompanySetting.UseLastExportDate : false;
            }
            set
            {
                if (_entity.InnerCompanySetting == null) return;
                _entity.InnerCompanySetting.UseLastExportDate = value;
                RaisePropertyChanged(() => this.UseLastExportDate);
            }
        }

        #endregion

        #region Constructors and PrivateMethods
        public UI_CompanySetting() { }
        public UI_CompanySetting(PMS_CompanySetting entity) : base(entity) { }

        public override void SetEntity(PMS_CompanySetting entity)
        {
            if (entity.InnerCompanySetting == null) entity.InnerCompanySetting = new PMS_InnerCompanySetting();
            base.SetEntity(entity);
            if (_entity.Logo != null && Logo == null)
            {
                Logo = new UI_File<PMS_CompanySettingLogoFileInfo>(_entity.Logo);
            }
        }
        #endregion      

        public override bool WasPropertiesValuesChanged
        {
            get
            {
                bool wasInnerSettingInfoChanged = (_entity.InnerCompanySetting != null)
                    ? (_entity.InnerCompanySetting.Id > 0) ? _entity.InnerCompanySetting.WasPropertiesValuesChanged : UseLastExportDate
                    : false;
                return base.WasPropertiesValuesChanged || wasInnerSettingInfoChanged || (this.Logo != null && this.Logo.WasPropertiesValuesChanged);
            }
        }


        public void Add(UI_File<PMS_CompanySettingLogoFileInfo> Logo)
        {
            PMS_CompanySettingLogoFileInfo logo = null;
            if (Logo != null)
            {
                logo = Logo.GetEntity();
            }
            _entity.AddComplex(logo);
        }

        public bool Save(UI_File<PMS_CompanySettingLogoFileInfo> Logo, UI_File<PMS_CompanySettingLogoFileInfo> LogoForDel)
        {
            PMS_CompanySettingLogoFileInfo logo = null;
            if (Logo != null)
            {
                logo = Logo.GetEntity();
            }
            PMS_CompanySettingLogoFileInfo logoForDel = null;
            if (LogoForDel != null)
            {
                logoForDel = LogoForDel.GetEntity();
            }
            return _entity.SaveComplex(logo, logoForDel);
        }

        public void LoadLogo()
        {
            if (Logo != null && Logo.FileDataBytes == null)
                ViewModels.Services.CompanySettingService.LoadFileData(this);
        }

        public override string this[string columnName]
        {
            get
            {
                /*switch (columnName)
                {
                    case "PeriodOfJobDataInputInPercent":
                        return (this.PeriodOfJobDataInputInPercent != null)
                        ? MVVMHelper.Validators.ValidatorVM.Int_CheckOnInRange(this.PeriodOfJobDataInputInPercent.Value, maxValue: 100)
                        : null;
                }*/
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DBServices.UsualEntity;
using Microsoft.Practices.Prism.Commands;
using ViewModelBaseSolutions.Items;

namespace Shared.ViewModels.Panels
{
    public class AdditionalPropertiesForVesselViewModel : Microsoft.Practices.Prism.ViewModel.NotificationObject
    {
        #region Properties
        public bool IsViewMode
        {
            get { return _IsViewMode; }
            set { _IsViewMode = value; RaisePropertyChanged(() => this.IsViewMode); }
        }
        bool _IsViewMode;
       
        public ObservableCollection<UI_ShipPropertyValue> ShipPropertyValues
        {
            get { return _ShipPropertyValues; }
            set { _ShipPropertyValues = value; RaisePropertyChanged(() => this.ShipPropertyValues); }
        }
        ObservableCollection<UI_ShipPropertyValue> _ShipPropertyValues;
        #endregion

        public AdditionalPropertiesForVesselViewModel()
        {
            IsViewMode = false;
            ShipPropertyValues = null;
        }
   
        public bool LoadForViewing(Ship ship)
        {
            IsViewMode = true;
            ShipPropertyValues = UI_ShipPropertyValue.GetPropertyValuesForShip(ship);
            return ShipPropertyValues != null;
        }

        public bool LoadForEditing(Ship ship)
        {
            ShipPropertyValues = UI_ShipPropertyValue.GetAllPropertyWithShipValues(ship);
            return ShipPropertyValues != null;
        }

        public bool LoadForAdding(Ship ship)
        {
            if (ship.PropertyValues == null) ship.PropertyValues = new ObservableCollection<ShipPropertyValue>();
            ShipPropertyValues = UI_ShipPropertyValue.GetAllProperty(ship);
            return ShipPropertyValues != null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBServices.ShipManager;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UIMessager.Services.Message;
using DBServices.UsualEntity;
using BaseLib.Services;

namespace Shared
{
    public class UI_ShipPropertyValue : NotifyPropertyChanged
    {
        #region Properties
        public ShipProperty ShipProperty { get; private set; }

        public ShipPropertyValue ShipValue { get; private set; }

        public Ship CurrentShip { get; private set; }
        #endregion


        public UI_ShipPropertyValue(ShipProperty property, ShipPropertyValue value, Ship ship)
        {
            ShipProperty = property;
            CurrentShip = ship;
            if (value == null)
            {
                ShipValue = new ShipPropertyValue { IdShipProperty = property.Id, IdShip = ship.Id };
                CurrentShip.PropertyValues.Add(ShipValue);
            }
            else ShipValue = value;
        }

        public static ObservableCollection<UI_ShipPropertyValue> GetAllPropertyWithShipValues(Ship ship)
        {
            ObservableCollection<UI_ShipPropertyValue> propertyValues = new ObservableCollection<UI_ShipPropertyValue>();
            ObservableCollection<ShipProperty> properties = ShipManager.GetShipProperties();
            if (properties == null) return null;
           
            foreach (var item in properties)
            {
                ShipPropertyValue value = ship.PropertyValues.Where(i => i.IdShipProperty == item.Id).FirstOrDefault();
                propertyValues.Add(new UI_ShipPropertyValue(item, value, ship));
            }

            return propertyValues;
        }

        public static ObservableCollection<UI_ShipPropertyValue> GetAllProperty(Ship ship)
        {
            ObservableCollection<UI_ShipPropertyValue> propertyValues = new ObservableCollection<UI_ShipPropertyValue>();
            ObservableCollection<ShipProperty> properties = ShipManager.GetShipProperties();
            if (properties == null) return null;

            foreach (var item in properties)
            {
                propertyValues.Add(new UI_ShipPropertyValue(item, null, ship));
            }

            return propertyValues;
        }

        public static ObservableCollection<UI_ShipPropertyValue> GetPropertyValuesForShip(Ship ship)
        {
            ObservableCollection<UI_ShipPropertyValue> propertyValues = new ObservableCollection<UI_ShipPropertyValue>();
            foreach (var item in ship.PropertyValues.Where(p => p.Value != null))
            {
                propertyValues.Add(new UI_ShipPropertyValue(item.Property, item, ship));
            }

            return propertyValues;
        }

    }
}

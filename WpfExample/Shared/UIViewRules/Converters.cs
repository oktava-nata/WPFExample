using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Shared.UIViewRules
{

    #region ChangeInfo: Who is Creator and Who is Editor

    [ValueConversion(typeof(bool), typeof(string))]
    public class EntityCreatorIsShipOrCompanyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is bool)) return global::Resources.Properties.Resources.txtCreated;

            bool WasCreateByOuter = (bool)value;

            string SelfTxt = null;
            string OuterTxt = null;
            if (BaseLib.AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Ship
                || BaseLib.AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentShip)
            {
                SelfTxt = global::Resources.Properties.ResourcesReplaced.txtCreatedByVessel;
                OuterTxt = Properties.Resources.txtCreatedByCompany;
            }
            else
            {
                SelfTxt = Properties.Resources.txtCreatedByCompany;
                OuterTxt = global::Resources.Properties.ResourcesReplaced.txtCreatedByVessel;
            }

            return (WasCreateByOuter) ? OuterTxt : SelfTxt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(string))]
    public class EntityEditorIsShipOrCompanyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is bool)) return null;

            bool ShowInfoForSelf = (bool)value;
            string SelfTxt = null;
            string OuterTxt = null;

            if (BaseLib.AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Ship
                || BaseLib.AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentShip)
            {
                SelfTxt = global::Resources.Properties.ResourcesReplaced.txtEditedByVessel;
                OuterTxt = Properties.Resources.txtEditedByCompany;
            }
            else
            {
                SelfTxt = Properties.Resources.txtEditedByCompany;
                OuterTxt = global::Resources.Properties.ResourcesReplaced.txtEditedByVessel;
            }

            return (ShowInfoForSelf) ? SelfTxt : OuterTxt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region EntityItem
    [ValueConversion(typeof(int), typeof(int?))]
    public class SelectEntityItemIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return (int)DBServices.TypeItem.None;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (int)value == (int)DBServices.TypeItem.None)
                return null;
            else return value;
        }
    }

    //Нигде пока не используеься
    //[ValueConversion(typeof(DBServices.dbEntity), typeof(DBServices.EntityItem<DBServices.dbEntity>))]
    //public class SelectEntityItemConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null) return new DBServices.EntityItem<DBServices.dbEntity>(DBServices.TypeItem.None, "");
    //        DBServices.dbEntity entity = value as DBServices.dbEntity;
    //        return new DBServices.EntityItem<DBServices.dbEntity>(entity);
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null || !(value is DBServices.EntityItem<DBServices.dbEntity>)) return null;
    //        DBServices.EntityItem<DBServices.dbEntity> SelectEntityItem = value as DBServices.EntityItem<DBServices.dbEntity>;
    //        if (SelectEntityItem.Type == DBServices.TypeItem.Specific)
    //            return SelectEntityItem.Entity;
    //        return null;
    //    }
    //}

    #endregion

    #region MRV
    //Olga
    //При null отображается значение по умолчанию
    [ValueConversion(typeof(int), typeof(string))]
    public class UnitOfCargoConverter : IValueConverter
    {
        public bool IsTakeAccountCultureInfo { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return default(DBServices.UnitOfCargoValue);
            switch ((DBServices.UnitOfCargoValue)value)
            {
                case DBServices.UnitOfCargoValue.t: return (IsTakeAccountCultureInfo) ? ResManager.GetResource(() => Properties.Resources.unit_of_cargo_t, culture) : Properties.Resources.unit_of_cargo_t;
                case DBServices.UnitOfCargoValue.m3: return (IsTakeAccountCultureInfo) ? ResManager.GetResource(() => Properties.Resources.unit_of_cargo_m3, culture) : Properties.Resources.unit_of_cargo_m3;
                case DBServices.UnitOfCargoValue.t_DWT: return (IsTakeAccountCultureInfo) ? ResManager.GetResource(() => Properties.Resources.unit_of_cargo_t_DWT, culture) : Properties.Resources.unit_of_cargo_t_DWT;
                case DBServices.UnitOfCargoValue.Passengers: return (IsTakeAccountCultureInfo) ? ResManager.GetResource(() => Properties.Resources.unit_of_cargo_Passengers, culture) : Properties.Resources.unit_of_cargo_Passengers;
                default: return "";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}

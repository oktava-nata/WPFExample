using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using DBServices;

namespace Shared.SpecialUnit
{  
    public class UnitListToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (values[0] == null)
                return "";

            decimal currentValue = decimal.Zero;
            decimal.TryParse(values[0].ToString(), out currentValue);

            currentValue = currentValue < 0 ? -currentValue : currentValue;

            SpecialUnitValue? unitType = values[1] != null && values[1] != System.Windows.DependencyProperty.UnsetValue ? (SpecialUnitValue?)values[1] : (SpecialUnitValue?)null;

            string otherUnitName = values[2] != null ? values[2].ToString() : "";

            var unitList = new ComplexNumber(unitType, currentValue, otherUnitName);
            return unitList.TextView;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    public class SpecialUnitPartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "";
            switch ((DBServices.SpecialUnitPartValue)value)
            {
                case DBServices.SpecialUnitPartValue.Day : return Properties.Resources.txtUnitD;
                case DBServices.SpecialUnitPartValue.Hour: return Properties.Resources.txtUnitH;
                case DBServices.SpecialUnitPartValue.Minute: return Properties.Resources.txtUnitM;
                case DBServices.SpecialUnitPartValue.Second: return Properties.Resources.txtUnitS;
                default: return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    
}

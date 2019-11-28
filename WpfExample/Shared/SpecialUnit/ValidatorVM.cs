using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.SpecialUnit
{
    public static class ValidatorVM
    {        
        /// <summary>
        /// Проверка сложного числа на максимально/минимально допустимое значение
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue">минимально допустимое значение числа</param>
        /// <param name="maxValue">максимально допустимое значение числа</param>
        /// <param name="canBeZero">возможность равняться нулю</param>
        /// <returns></returns>
        public static string ComplexNumber_CheckOnInRange(ComplexNumber value, decimal minValue = default(decimal), decimal maxValue = decimal.MaxValue, bool canBeZero = true)
        {       
            if(value.SpecialUnitType == null)
                return MVVMHelper.Validators.ValidatorVM.Decimal_CheckOnInRange
                                (value: value.BaseValue.Value
                                , minValue: minValue
                                , maxValue: maxValue
                                , canBeZero: canBeZero);

            if (value.BaseValue < minValue)
                return string.Format(Properties.Resources.txtEnterComplexNumberValueNotLess
                    , new UnitListToStringConverter().Convert(new object[] { minValue, value.SpecialUnitType, "" }, null, null, null).ToString());
            if (value.BaseValue > maxValue)
                return string.Format(Properties.Resources.txtEnterComplexNumberValueNotMore
                    , new UnitListToStringConverter().Convert(new object[] { maxValue, value.SpecialUnitType, "" }, null, null, null).ToString());
            if (minValue <= 0 && !canBeZero && (value.BaseValue.Value.CompareTo(0) == 0)) return Properties.Resources.txtEnterComplexNumberValueNotZero;

            return null;
        }        
    }
}

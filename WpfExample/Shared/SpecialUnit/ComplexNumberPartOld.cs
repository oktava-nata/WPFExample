using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Shared.SpecialUnit
{
    public class ComplexNumberPartOld : BaseLib.Services.NotifyPropertyChanged, IDataErrorInfo
    {
        public delegate void ValuePartChangedEventHandler(bool newValuePartIsNotNull);
        public event ValuePartChangedEventHandler ValuePartChanged;

        #region Properties
        /// <summary>
        /// Название составной части единицы измерения
        /// </summary>
        public string UnitPartName { get; private set; }

        /// <summary>
        /// Отношение базового значения к составной единице измерения, т.е. то число, 
        /// на которое нужно разделить базовое число
        /// </summary>
        public int Quotient { get; private set; }

        private int? _value;
        public int? Value
        {
            get { return _value; }
            set
            {
                bool isChanged = _value != value;
                _value = value;
                RaisePropertyChanged(() => this.Value);
                if (isChanged && (!_value.HasValue || _value <= MaxValue))//
                    ValuePartChanged(_value.HasValue);
            }
        }

        private int _maxValue;
        public int MaxValue { get { return _maxValue; } }

        private bool _canBeNull;
        #endregion

        public ComplexNumberPartOld(string unitPartName, int quotient, int maxValue, bool canBeNull, ValuePartChangedEventHandler onValuePartChanged)
        {
            UnitPartName = unitPartName;
            Quotient = quotient;
            _maxValue = maxValue;
            _canBeNull = canBeNull;
            ValuePartChanged = onValuePartChanged;
        }

        public void SetValuePart(int? partValue)
        {
            _value = partValue;
            RaisePropertyChanged(() => this.Value);
        }

        #region IDataErrorInfo
        public string Error
        {
            get { return null; }
        }

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case "Value":
                    Resources.Validators.NotInRangeIntRule validator1 = new Resources.Validators.NotInRangeIntRule();
                    validator1.Max = MaxValue;
                    validator1.CanBeNUll = _canBeNull;
                    System.Windows.Controls.ValidationResult result1 =
                        validator1.Validate(_value, null);
                    return (!result1.IsValid) ? result1.ErrorContent.ToString() : null;
                    default: return null;
                }
            }
        }

        public virtual bool IsValid()
        {
            foreach (var property in this.GetType().GetProperties())
            {
                if ((this as System.ComponentModel.IDataErrorInfo)[property.Name] != null)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}

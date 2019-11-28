using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Shared.SpecialUnit
{
    public class ComplexNumberPart : BaseLib.Services.NotifyPropertyChanged, IDataErrorInfo
    {
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
                if (isChanged && (!_value.HasValue || _value <= MaxValue))
                    ValuePartChanged();
            }
        }

        private int _maxValue;
        public int MaxValue { get { return _maxValue; } }

        public Action ValuePartChanged;
        #endregion

        public ComplexNumberPart(string unitPartName, int quotient, int maxValue, Action onValuePartChanged)
        {
            UnitPartName = unitPartName;
            Quotient = quotient;
            _maxValue = maxValue;
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
                        return (_value.HasValue)
                            ? MVVMHelper.Validators.ValidatorVM.Int_CheckOnInRange(value: _value.Value, maxValue: MaxValue)
                            : null;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DBServices;

namespace Shared.SpecialUnit
{
    public class ComplexNumberOld : BaseLib.Services.NotifyPropertyChanged
    {
        readonly int MAX_PART_VALUE = 100000000;

        public delegate void BaseValueChangedEventHandler(decimal? newValue);
        public event BaseValueChangedEventHandler BaseValueChanged;

        #region Properties
        private decimal? _baseValue;
        public decimal? BaseValue
        {
            get { return _baseValue; }
            set
            {
                //базовое значение было null, а теперь стало не null
                bool baseValueBecomeNotNull = !_baseValue.HasValue && value.HasValue;
                _baseValue = value;
                RaisePropertyChanged(() => this.BaseValue);

                if (BaseValueChanged != null)
                {
                    if (!(!CanBeNull && !BaseValue.HasValue) && !(!CanBeZero && BaseValue.HasValue && BaseValue.Value == 0))
                        BaseValueChanged(BaseValue);
                }

                GeneratePartValues(baseValueBecomeNotNull);
            }
        }

        private ObservableCollection<ComplexNumberPartOld> _complexNumberPartList;
        public ObservableCollection<ComplexNumberPartOld> ComplexNumberPartList
        {
            get { return _complexNumberPartList; }
            private set
            {
                _complexNumberPartList = value;
                RaisePropertyChanged(() => this.ComplexNumberPartList);
            }
        }

        private SpecialUnitValue? _specialUnitType;
        public SpecialUnitValue? SpecialUnitType
        {
            get { return _specialUnitType; }
            private set
            {
                _specialUnitType = value;
                CreateComplexNumberPartListByUnitType();
                RaisePropertyChanged(() => this.SpecialUnitType);
            }
        }

        private bool _canBeNull;
        private bool _canBeZero;

        public bool CanBeNull { get { return _canBeNull; } }
        public bool CanBeZero { get { return _canBeZero; } }

        private string _otherUnitName;
        public string OtherUnitName
        {
            get { return _otherUnitName; }
            private set
            {
                _otherUnitName = value;
                RaisePropertyChanged(() => this.OtherUnitName);
            }
        }


        public string TextView
        {
            get { return GetTextView(); }
        }
        #endregion

        #region Constructors
        public ComplexNumberOld(SpecialUnitValue? unitType, decimal? currentValue, string otherUnitName, bool canBeZero, bool canBeNull, BaseValueChangedEventHandler onBaseValueChanged = null)
        {
            _baseValue = currentValue;
            _otherUnitName = otherUnitName;
            _canBeNull = canBeNull;
            SpecialUnitType = unitType;

            _canBeZero = canBeZero;
            BaseValueChanged = onBaseValueChanged;
        }

        public ComplexNumberOld(SpecialUnitValue? unitType, decimal? currentValue, string otherUnitName)
        {
            _baseValue = currentValue;
            _otherUnitName = otherUnitName;
            SpecialUnitType = unitType;
        }
        #endregion

        public void SetSpecialUnitType(SpecialUnitValue? unitType)
        {
            SpecialUnitType = unitType;
        }

        public static string ToText(decimal? value, SpecialUnitValue? unitType, string otherUnitName)
        {
            return new UnitListToStringConverter().Convert(new object[] { value, unitType, otherUnitName }, null, null, null).ToString();
        }

        #region private Method
        private void CreateComplexNumberPartListByUnitType()
        {
            var list = new ObservableCollection<ComplexNumberPartOld>();
            if (SpecialUnitType.HasValue)
            {
                var parts = DBServices.SpecialUnitGenerator.Generate(SpecialUnitType.Value);
                if (parts != null)
                {
                    var enumerator = parts.GetEnumerator();
                    bool hasNext = enumerator.MoveNext();
                    if (hasNext)
                    {
                        var reverseList = new List<ComplexNumberPartOld>();
                        KeyValuePair<SpecialUnitPartValue, int> currentItem = enumerator.Current;
                        KeyValuePair<SpecialUnitPartValue, int> lastItem;
                        int globalQuotient = currentItem.Value;
                        do
                        {
                            hasNext = enumerator.MoveNext();
                            lastItem = currentItem;
                            currentItem = enumerator.Current;
                            int maxValue = !hasNext ? MAX_PART_VALUE : currentItem.Value - 1;
                            reverseList.Add(new ComplexNumberPartOld(new SpecialUnitPartConverter().Convert(lastItem.Key, null, null, null).ToString(), globalQuotient, maxValue, _canBeNull, GenerateNewBaseValue));
                            globalQuotient = hasNext ? (globalQuotient * currentItem.Value) : globalQuotient;
                        } while (hasNext);

                        reverseList.Reverse();
                        reverseList.ForEach(i => list.Add(i));
                    }
                }
            }

            ComplexNumberPartList = list;

            //после смены типа переменной нужно в любом случае проставить разряды, т.к. сами разрядности изменились
            GeneratePartValues(true);
        }

        /// <summary>
        /// Генерируем составное число из базвого
        /// </summary>
        private void GeneratePartValues(bool baseValueBecomeNotNull)
        {
            decimal? baseValue = BaseValue;
            foreach (var item in ComplexNumberPartList)
            {
                if (item.Quotient == 0)
                    return;

                int? newPartValue = (int?)(baseValue / item.Quotient);

                item.SetValuePart(item.Value.HasValue || baseValueBecomeNotNull ? newPartValue : null);
                if (item.Value.HasValue)
                    baseValue -= (decimal)item.Value * item.Quotient;
            }
            RaisePropertyChanged(() => this.TextView);
            RaisePropertyChanged(() => this.ComplexNumberPartList);
        }

        /// <summary>
        /// генерируем базовое число из составного
        /// </summary>
        private void GenerateNewBaseValue(bool newValuePartIsNotNull)
        {
            decimal baseValue = 0;
            bool hasNotNullablePart = false;
            foreach (var item in ComplexNumberPartList)
            {
                if (item.Quotient == 0)
                    return;
                if (item.Value.HasValue)
                {
                    baseValue += (decimal)Math.Min(item.Value.Value, item.MaxValue) * item.Quotient;
                    hasNotNullablePart = true;
                }
                else if (newValuePartIsNotNull)
                {
                    item.SetValuePart(0);
                }
            }
            BaseValue = hasNotNullablePart ? baseValue : (decimal?)null;

            //if (BaseValueChanged != null)
            //    BaseValueChanged(BaseValue);
        }

        private string GetTextView()
        {
            if (BaseValue.HasValue)
            {
                if (!SpecialUnitType.HasValue)
                {
                    return String.Format(" {0} {1}",
                        new Converters.ToStringFormat.DecimalToStringConverter().Convert(BaseValue, null, null, null),
                        _otherUnitName);
                }

                string text = "";
                foreach (var item in ComplexNumberPartList)
                    text += String.Format(" {0} {1}", item.Value, item.UnitPartName);

                return text;
            }
            return "";
        }
        #endregion

        public bool HasNullAndNotNullValues()
        {
            //для простого числа проверяем введено ли значение, если это требуется
            if (!SpecialUnitType.HasValue)
                return !BaseValue.HasValue && !CanBeNull;
            //для сложного числа проверяем на то, что данные введены частично (есть заполненные и не заполненные ячейки)
            if (ComplexNumberPartList == null)
                return false;
            return ComplexNumberPartList.Where(i => i.Value.HasValue).Count() > 0
                && ComplexNumberPartList.Where(i => !i.Value.HasValue).Count() > 0;
        }

        internal bool IsZero()
        {
            if (ComplexNumberPartList == null)
                return false;
            return ComplexNumberPartList.Where(i => !i.Value.HasValue).Count() == 0 && BaseValue == 0;
        }

        public bool IsNull()
        {
            //для простого числа проверяем базовое значение
            if (!SpecialUnitType.HasValue)
                return !BaseValue.HasValue;
            if (ComplexNumberPartList == null)
                return true;
            return ComplexNumberPartList.Where(i => i.Value.HasValue).Count() == 0;
        }

        /// <summary>
        /// Проверка правильности ввода
        /// </summary>
        public bool IsValidEnter()
        {
            //для простого числа проверяем введено ли значение, если это требуется
            if (!SpecialUnitType.HasValue)
                return !BaseValue.HasValue && !CanBeNull;

            //для сложного числа проверяем на то, что каждое поле заполненно правильно
            if (ComplexNumberPartList == null)
                return false;
            bool hasNotValid = ComplexNumberPartList.Where(i => !i.IsValid()).Any();
            return !hasNotValid;

        }


        internal void Clear()
        {
            BaseValue = null;
        }

        internal void SetOtherUnitName(string otherUnitName)
        {
            OtherUnitName = otherUnitName;
        }
    }


}

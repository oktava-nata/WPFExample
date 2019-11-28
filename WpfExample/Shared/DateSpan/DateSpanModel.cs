using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Shared.DateSpan
{
    public class DateSpanModel : BaseLib.Services.NotifyPropertyChanged, IDataErrorInfo
    {
        private IDateSpan DateSpan;
        private bool _canBeNull;
        private bool _canBeZero;

        public DateSpanModel(IDateSpan dateSpan, bool canBeZero, bool canBeNull)
        {
            DateSpan = dateSpan;
            _canBeZero = canBeZero;
            _canBeNull = canBeNull;
        }

        public bool CanBeNull { get { return _canBeNull; } }
        public bool CanBeZero { get { return _canBeZero; } }

        public int? Days
        {
            get { return DateSpan.Value_inDays; }
            set
            {
                if (DateSpan.Value_inDays != value)
                {
                    DateSpan.Value_inDays = value;
                    RaisePropertyChanged(() => this.Days);
                    if (value.HasValue)
                        setZeroIfNull();
                }
            }
        }
        public int? Months
        {
            get { return DateSpan.Value_inMonths; }
            set
            {
                if (DateSpan.Value_inMonths != value)
                {
                    DateSpan.Value_inMonths = value;
                    RaisePropertyChanged(() => this.Months);
                    if (value.HasValue)
                        setZeroIfNull();
                }
            }
        }
        public int? Years
        {
            get { return DateSpan.Value_inYears; }
            set
            {
                if (DateSpan.Value_inYears != value)
                {
                    DateSpan.Value_inYears = value;
                    RaisePropertyChanged(() => this.Years);
                    if (value.HasValue)
                        setZeroIfNull();
                }
            }
        }

        private void setZeroIfNull()
        {
            if (!DateSpan.Value_inDays.HasValue)
            {
                DateSpan.Value_inDays = 0;
                RaisePropertyChanged(() => this.Days);
            }

            if (!DateSpan.Value_inMonths.HasValue)
            {
                DateSpan.Value_inMonths = 0;
                RaisePropertyChanged(() => this.Months);
            }

            if (!DateSpan.Value_inYears.HasValue)
            {
                DateSpan.Value_inYears = 0;
                RaisePropertyChanged(() => this.Years);
            }
        }

        #region Check
        public bool IsNull()
        {
            return !Days.HasValue && !Months.HasValue && !Years.HasValue;
        }

        public bool HasNullAndNotNullValues()
        {
            bool hasNullValue = !Days.HasValue || !Months.HasValue || !Years.HasValue;
            bool hasNotNullValue = Days.HasValue || Months.HasValue || Years.HasValue;
            return hasNullValue && hasNotNullValue;
        }

        public bool IsZero()
        {
            return (Days.HasValue && Days.Value == 0)
                && (Months.HasValue && Months.Value == 0)
                && (Years.HasValue && Years.Value == 0);
        }
        #endregion

        #region Methods
        public void Clear()
        {
            Days = Months = Years = null;
        }
        #endregion

        #region IDataErrorInfo
        public string Error
        {
            get { return null; }
        }

        public string this[string propertyName]
        {
            get
            {
                //if (_canBeNull)
                //    return null;

                var validator = new Resources.Validators.NotInRangeIntRule() { CanBeNUll = _canBeNull, Max = 1000 };

                switch (propertyName)
                {
                    case "Days":
                        var result = validator.Validate(Days, null);
                        return (!result.IsValid) ? result.ErrorContent.ToString() : null;
                    case "Months":
                        var result1 = validator.Validate(Months, null);
                        return (!result1.IsValid) ? result1.ErrorContent.ToString() : null;
                    case "Years":
                        var result2 = validator.Validate(Years, null);
                        return (!result2.IsValid) ? result2.ErrorContent.ToString() : null;
                    default: return null;
                }
            }
        }
        #endregion


    }
}

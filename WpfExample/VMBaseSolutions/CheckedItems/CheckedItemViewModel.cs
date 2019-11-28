using System;

namespace VMBaseSolutions.CheckedItems
{
    public class CheckedItemViewModel<T> : Telerik.Windows.Controls.ViewModelBase
    {
        public Action OnCheckedChanged;

        #region Properties
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                OnPropertyChanged(() => this.IsChecked);
                if (OnCheckedChanged != null)
                    OnCheckedChanged();
            }
        }
        bool _IsChecked;

        public bool IsAvailableForCheck
        {
            get { return _IsAvailableForCheck; }
            set
            {
                _IsAvailableForCheck = value;
                OnPropertyChanged(() => this.IsAvailableForCheck);
            }
        }
        bool _IsAvailableForCheck;

        public T ItemObject { get; set; }
        #endregion

        public CheckedItemViewModel(T itemObject, bool isChecked, Action onCheckedChanged, bool isAvailableForCheck = true)
        {
            ItemObject = itemObject;
            IsChecked = isChecked;
            OnCheckedChanged = onCheckedChanged;
            IsAvailableForCheck = isAvailableForCheck;
        }

        public CheckedItemViewModel(T itemObject, bool isAvailableForCheck = true)
        {
            ItemObject = itemObject;
            IsAvailableForCheck = isAvailableForCheck;
        }

        public override string ToString()
        {
            return ItemObject.ToString();
        }
    }


}

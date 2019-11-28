using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using Telerik.Windows.Controls;
using VMBaseSolutions.CheckedItems;

namespace Controls
{
    public class MultiSelectPanelViewModel<T> : ViewModelBase
    {
        #region Commands
        public DelegateCommand SelectCommand { get; private set; }
        public DelegateCommand OpenCommand { get; private set; }
        public DelegateCommand ClearCommand { get; private set; }
        #endregion

        #region Properties
        public string SelectedText
        {
            get { return _SelectedText; }
            private set { _SelectedText = value; OnPropertyChanged(() => this.SelectedText); }
        }
        string _SelectedText;

        public ListCollectionView ItemList
        {
            get { return _ItemList; }
            private set { _ItemList = value; OnPropertyChanged(() => this.ItemList); }
        }
        ListCollectionView _ItemList;

        public bool ShowPopup
        {
            get { return _ShowPopup; }
            set
            {
                if (!value && value != _ShowPopup)
                {
                    UpdateText();
                    if (_OnChangeSelectedItems != null) _OnChangeSelectedItems();
                }
                _ShowPopup = value; OnPropertyChanged(() => this.ShowPopup);
            }
        }
        bool _ShowPopup;

        public List<T> SelectedItems { get; private set; }

        ListCheckedItemViewModel<T> _CheckedList;

        Action _OnChangeSelectedItems;
        #endregion


        public MultiSelectPanelViewModel(List<T> sourceList, List<T> defaultSelectList, Action onChangeSelectedItems)
        {
            _OnChangeSelectedItems = onChangeSelectedItems;

            SelectCommand = new DelegateCommand(this.OnSelect, delegate (object obj) { return _CheckedList != null && _CheckedList.Selected > 0; });
            OpenCommand = new DelegateCommand(this.OnOpen);
            ClearCommand = new DelegateCommand(this.OnClear, delegate (object obj) { return _CheckedList != null && _CheckedList.Selected > 0; });

            _CheckedList = new ListCheckedItemViewModel<T>(sourceList, false, OnSelectionChanged);

            SetDefaultSelectList(defaultSelectList);

            ItemList = new ListCollectionView(_CheckedList);

            UpdateText();
        }

        public MultiSelectPanelViewModel(List<CheckedItemViewModel<T>> sourceList, List<T> defaultSelectList, Action onChangeSelectedItems)
        {
            _OnChangeSelectedItems = onChangeSelectedItems;

            SelectCommand = new DelegateCommand(this.OnSelect, delegate (object obj) { return _CheckedList != null && _CheckedList.Selected > 0; });
            OpenCommand = new DelegateCommand(this.OnOpen);
            ClearCommand = new DelegateCommand(this.OnClear, delegate (object obj) { return _CheckedList != null && _CheckedList.Selected > 0; });

            _CheckedList = new ListCheckedItemViewModel<T>(sourceList, false, OnSelectionChanged);

            SetDefaultSelectList(defaultSelectList);

            ItemList = new ListCollectionView(_CheckedList);

            UpdateText();
        }

        /// <summary>
        /// Предвыделение
        /// </summary>
        /// <param name="defaultSelectList"></param>
        public void SetDefaultSelectList(List<T> defaultSelectList)
        {
            //снимаем выделение
            _CheckedList.UncheckedAll();
            //новое выделение
            if (defaultSelectList != null)
                foreach (var item in defaultSelectList)
                {
                    var sel = _CheckedList.Where(i => i.ItemObject.Equals(item)).FirstOrDefault();
                    if (sel != null) sel.IsChecked = true;
                }

            UpdateText();
        }

        public void Clear()
        {
            if (_CheckedList.Selected > 0)
            {
                _CheckedList.UncheckedAll();
                UpdateText();
            }
        }

        void OnSelectionChanged()
        {
            this.SelectCommand.InvalidateCanExecute();
            this.ClearCommand.InvalidateCanExecute();
        }

        private void UpdateText()
        {
            SelectedItems = _CheckedList.GetSelectedTList();
            if (SelectedItems == null || SelectedItems.Count == 0)
                SelectedText = Resources.Properties.Resources.txtNotSelected;
            else
            {
                var names = SelectedItems.Select(i => i.ToString()).ToList();
                SelectedText = string.Join(", ", names);
            }
        }

        #region On Command
        void OnSelect(object obj)
        {
            ShowPopup = false;
        }

        void OnOpen(object obj)
        {
            ShowPopup = true;
        }

        void OnClear(object obj)
        {
            _CheckedList.UncheckedAll();
            UpdateText();
            if (_OnChangeSelectedItems != null) _OnChangeSelectedItems();
        }
        #endregion
    }
}

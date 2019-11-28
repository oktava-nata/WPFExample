using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Shared.DateSpan
{
    public class DateSpanViewModel : BaseLib.Services.NotifyPropertyChanged
    {
        #region Properties
        public DateSpanModel Model { get; set; }

        public DelegateCommand ClearCommand { get; private set; }

        private string _errorText;
        public string ErrorText
        {
            get { return _errorText; }
            private set
            {
                _errorText = value;
                RaisePropertyChanged(() => this.ErrorText);
            }
        } 
        #endregion

        public DateSpanViewModel(IDateSpan dateSpan, bool canBeZero, bool canBeNull)
        {
            Model = new DateSpanModel(dateSpan, canBeZero, canBeNull);
            Model.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Model_PropertyChanged);
            this.ClearCommand = new DelegateCommand(this.OnClear, this.CanClear);            
        }

        internal bool HasErrors()
        {
            if (!Model.CanBeZero && Model.IsZero())
            {
                ErrorText = string.Format(global::Resources.Properties.Resources.txtEnterMoreValue, 0);
                return true;
            }
            if (Model.HasNullAndNotNullValues())
            {
                ErrorText = global::Resources.Properties.Resources.txtEnterData;
                return true;
            }

            ErrorText = null;
            return false;
        }

        private void OnClear() { this.Model.Clear(); }
        private bool CanClear() { return !this.Model.IsNull(); }

        void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ErrorText = null;
            this.ClearCommand.RaiseCanExecuteChanged();
        }

    }
   
}

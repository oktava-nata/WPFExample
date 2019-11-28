using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using DBServices;

namespace Shared.SpecialUnit
{
    public class SpecialUnitViewModelOld: BaseLib.Services.NotifyPropertyChanged
    {
        public delegate bool ValidateMethod(out string errorText, ComplexNumberOld complexNumber);
        public event ValidateMethod AdditionalValidate;

        #region Properties
        public ComplexNumberOld ComplexNumberForEdit { get; set; }

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

        public SpecialUnitViewModelOld(SpecialUnitValue? unitType, decimal? currentValue, string otherUnitName, bool canBeZero, bool canBeNull, Shared.SpecialUnit.ComplexNumberOld.BaseValueChangedEventHandler onBaseValueChanged)
        {
            ComplexNumberForEdit = new ComplexNumberOld(unitType, currentValue, otherUnitName, canBeZero, canBeNull, onBaseValueChanged);
            ComplexNumberForEdit.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Model_PropertyChanged);
            this.ClearCommand = new DelegateCommand(this.OnClear, this.CanClear);            
        }

        internal bool HasErrors()
        {
           
            //выполняем стандартную проверку на возможноть 0
            if (!ComplexNumberForEdit.CanBeZero && ComplexNumberForEdit.IsZero())
            {
                ErrorText = string.Format(global::Resources.Properties.Resources.txtEnterMoreValue, 0);
                return true;
            }
            //выполняем стандартную проверку на возможноть Null
            if (ComplexNumberForEdit.HasNullAndNotNullValues())
            {
                ErrorText = global::Resources.Properties.Resources.txtEnterData;
                return true;
            }

            if (AdditionalValidate == null)
            {
                ErrorText = null;
                return false;
            }
            //выполняем дополнительную проверку
            string errorText;
            bool result = AdditionalValidate(out errorText, ComplexNumberForEdit);
            ErrorText = errorText;

            return result;            
        }

        private void OnClear()
        {
            this.ComplexNumberForEdit.Clear(); 
        }
        private bool CanClear()
        {
            return !this.ComplexNumberForEdit.IsNull(); 
        }

        void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ErrorText = null;
            this.ClearCommand.RaiseCanExecuteChanged();
        }
    }
}

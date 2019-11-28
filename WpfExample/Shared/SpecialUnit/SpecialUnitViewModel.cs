using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using DBServices;
using System.ComponentModel;

namespace Shared.SpecialUnit
{
    public class SpecialUnitViewModel : BaseLib.Services.NotifyPropertyChanged, IDataErrorInfo
    {
        #region Properties
        public ComplexNumber ComplexNumberForEdit { get; set; }

        public DelegateCommand ClearCommand { get; private set; }
        #endregion

        public SpecialUnitViewModel(SpecialUnitValue? unitType, decimal? currentValue, string otherUnitName, bool canBeNull, Shared.SpecialUnit.ComplexNumber.BaseValueChangedEventHandler onBaseValueChanged)
        {
            ComplexNumberForEdit = new ComplexNumber(unitType, currentValue, otherUnitName, true, canBeNull, onBaseValueChanged);
            ComplexNumberForEdit.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Model_PropertyChanged);
            this.ClearCommand = new DelegateCommand(this.OnClear, this.CanClear);
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
            this.ClearCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(()=>this.ComplexNumberForEdit);
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "ComplexNumberForEdit":
                        if (!ComplexNumberForEdit.CanBeNull && ComplexNumberForEdit.IsNull())
                            return global::Resources.Properties.Resources.txtEnterData;
                        return null;
                    default: return null;
                }
            }
        }
    }
}

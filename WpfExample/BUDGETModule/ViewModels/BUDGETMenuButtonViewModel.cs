using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using UIMessager.Services.Message;

namespace BUDGETModule.ViewModels
{
    public class BUDGETMenuButtonViewModel : Telerik.Windows.Controls.ViewModelBase
    {

        #region Commands
        public DelegateCommand ShowSuppliersViewCommand { get; private set; }
        public DelegateCommand ShowCurrencyViewCommand { get; private set; }
        #endregion



        #region Constructor
        public BUDGETMenuButtonViewModel()
        {
            InitializeVM();
        }

        private void InitializeVM()
        {
            ShowSuppliersViewCommand = new DelegateCommand(ShowSuppliersViewCommand_Executed);
            ShowCurrencyViewCommand = new DelegateCommand(ShowCurrencyViewCommand_Executed);
        }
        #endregion



        #region On Command

        private void ShowSuppliersViewCommand_Executed(object obj)
        {
            ViewCreator.SuppliersView_Show();

        }
        private void ShowCurrencyViewCommand_Executed(object obj)
        {
            ViewCreator.CurrencyView_Show();
        }

        #endregion
    }
}

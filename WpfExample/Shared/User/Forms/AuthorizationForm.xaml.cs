using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Resources.Validators;
using UIMessager.Services.Message;

namespace Shared.User.Forms
{
    /// <summary>
    /// Логика взаимодействия для AutorizationForm.xaml
    /// </summary>
    public partial class AuthorizationForm : Window
    {
        public bool LogIn { get { return _LogIn; } }
        bool _LogIn = false;

        public string pU
        {
            set { TxtPass.Password = value; }
        }

        public bool IsSavePsw
        {
            get { return chkSavePass.IsChecked.Value; }
            set { chkSavePass.IsChecked = value; }
        }

        public UI_User LoginUser
        {
            get { return (UI_User)DataContext; }
            set { DataContext = value; }
        }

        public AuthorizationForm()
        {
            InitializeComponent();
            LoginUser = new UI_User();
            var settings = CurrentUser_Settings.GetCurrentSettings();
            if (settings != null)
                LoginUser.Login = settings.LastLogin;

            if (LoginUser.Login != null && LoginUser.Login.Trim() != string.Empty)
                TxtPass.Focus();
            else TxtLogin.Focus();
            BtnLogin.IsDefault = true;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!FormHasErrors())
            {
                LoginUser.Password = TxtPass.Password;

                if ((_LogIn = LoginUser.LogIn()))
                {
                    Module.SetUserAndAccess(LoginUser);
                    if (IsSavePsw) User.UserLoader.SavePUser(TxtPass.Password);
                    else User.UserLoader.SavePUser(null);
                    Close();
                }
                else
                    MsgError.Show(Properties.Resources.mErInvalidLogOrPsw);
            }
        }


        private void TxtPass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtPass.Password.Length == 0)
                TxtPass.Style = (Style)(App.Current.Resources["PasswordBoxError"]);
            else
                TxtPass.Style = (Style)(App.Current.Resources["PasswordBoxOk"]);
        }

        private bool FormHasErrors()
        {
            if (TxtPass.Password.Length < 1)
            {
                TxtPass.Style = (Style)(App.Current.Resources["PasswordBoxError"]);
            }

            if (ValidateHelper.HasErrors(EditFields, true) || TxtPass.Password.Length < 1)
            {
                MsgError.Show(global::UIMessager.Properties.Resources.mErIncorrectInputDate);
                return true;
            }
            return false;
        }
    }
}

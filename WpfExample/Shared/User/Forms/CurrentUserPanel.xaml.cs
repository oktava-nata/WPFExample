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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Shared.Services;
using UIMessager.Services.Message;
using BaseLib;

namespace Shared.User.Forms
{
    /// <summary>
    /// Логика взаимодействия для CurrentUserPanel.xaml
    /// </summary>
    public partial class CurrentUserPanel : UserControl
    {
        public Func<bool> OnLogOutting;

        UI_User CurrentUser { get { return AppManager.CurrentUser as UI_User; } }

        public CurrentUserPanel()
        {
            InitializeComponent();
        }

        public void Load()
        {
            DataContext = CurrentUser;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            UserForm userF = new UserForm(CurrentUser);
            if (userF.FormIsLoaded)
            {
                userF.ShowDialog();                
            }
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            if (OnLogOutting == null || OnLogOutting())
                Module.LogOut();
        }
    }
}

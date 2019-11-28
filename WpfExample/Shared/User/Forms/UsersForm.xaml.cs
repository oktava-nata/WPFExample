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
using Shared.Services;
using BaseLib;

namespace Shared.User.Forms
{
    /// <summary>
    /// Логика взаимодействия для UsersForm.xaml
    /// </summary>
    public partial class UsersForm : Window
    {
        public UsersForm()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Module.UserAccess.IsUsers_managed)
            {
               if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                    if (!UserListPanel.Load())
                        Close();
            }
            else AppManager.WriteLog("User haven't access to form 'UsersForm' but he tried to open it. ");
        }
    }
}

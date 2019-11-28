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

namespace Shared.User.Forms
{
    /// <summary>
    /// Логика взаимодействия для CurrentUserPanel.xaml
    /// </summary>
    public partial class UnauthorizedUserPanel : UserControl
    {

        public UnauthorizedUserPanel()
        {
            InitializeComponent();
        }
       
        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            Module.LogOut();
        }
    }
}

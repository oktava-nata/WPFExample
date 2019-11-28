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

namespace Shared.Forms.Panels
{
    /// <summary>
    /// Логика взаимодействия для CompanyNamePanel.xaml
    /// </summary>
    public partial class CompanyNamePanel : UserControl
    {
        public CompanyNamePanel()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                companyName.Text = BaseLib.AppManager.CommonInfo.CompanyName;
            }
            else companyName.Text = "CompanyName";
        }
    }
}

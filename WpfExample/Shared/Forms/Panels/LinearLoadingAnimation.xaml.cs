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
    /// Логика взаимодействия для LinearLoadingAnimation.xaml
    /// </summary>
    public partial class LinearLoadingAnimation : UserControl
    {       
        public static DependencyProperty TextInformationProperty = DependencyProperty.Register("TextInformation", typeof(string), typeof(LinearLoadingAnimation));
        public string TextInformation
        {
            get { return (string)GetValue(TextInformationProperty); }
            set { SetValue(TextInformationProperty, value); }
        }

        public LinearLoadingAnimation()
        {
            InitializeComponent();
            TextInformation = global::UIMessager.Properties.Resources.txtWaiting;
        }
    }
}

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
    /// Логика взаимодействия для LoadingAnimation.xaml
    /// </summary>
    public partial class LoadingAnimation : UserControl
    {
        #region Dependency Properties
        public static DependencyProperty TextInformationProperty = DependencyProperty.Register("TextInformation", typeof(string), typeof(LoadingAnimation));
        public string TextInformation
        {
            get { return (string)GetValue(TextInformationProperty); }
            set { SetValue(TextInformationProperty, value); }
        }

        public static DependencyProperty ShowBackgroundProperty = DependencyProperty.Register("ShowBackground", typeof(bool), typeof(LoadingAnimation));
        public bool ShowBackground
        {
            get { return (bool)GetValue(ShowBackgroundProperty); }
            set { SetValue(ShowBackgroundProperty, value); }
        }        
        #endregion

        public LoadingAnimation()
        {
            InitializeComponent();
            TextInformation = global::UIMessager.Properties.Resources.txtWaiting;
        }
    }
}

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
using BaseLib.AdditionalTypes;

namespace Shared.Forms.Panels
{
    /// <summary>
    /// Логика взаимодействия для MonthPanel.xaml
    /// </summary>
    public partial class MonthPanel : UserControl
    {      
        #region Dependency Properties
        public static DependencyProperty PeriodProperty = DependencyProperty.Register
        (
            "Period",
            typeof(MonthPeriod),
            typeof(MonthPanel),
            new PropertyMetadata(new MonthPeriod())
        );
        #endregion

        public MonthPeriod Period
        {
            get { return (MonthPeriod)GetValue(PeriodProperty); }
            private set { SetValue(PeriodProperty, value); }
        }
       
        public MonthPanel()
        {
            InitializeComponent();
            Period = new MonthPeriod();
        }

        #region Btn_Click
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            var date = new DateTime(Period.Year, Period.Month, 1);
            var newDate = date.AddMonths(1);
            Period.SetDisplayYearAndMonth(newDate.Year, newDate.Month);
        }

        private void BtnLast_Click(object sender, RoutedEventArgs e)
        {
            var date = new DateTime(Period.Year, Period.Month, 1);
            var newDate = date.AddMonths(-1);
            Period.SetDisplayYearAndMonth(newDate.Year, newDate.Month);
        }
        #endregion

        
    }
}

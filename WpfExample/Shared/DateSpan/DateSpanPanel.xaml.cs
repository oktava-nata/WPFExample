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
using Resources.Validators;
using BaseLib;

namespace Shared.DateSpan
{
    /// <summary>
    /// Логика взаимодействия для DateSpanPanel.xaml
    /// </summary>
    public partial class DateSpanPanel : UserControl
    {
        #region DependencyProperties
        //TODO убрать после того как панель будет использоваться только в MVVM
        public static DependencyProperty DateSpanProperty = DependencyProperty.Register
         (
             "DateSpan",
             typeof(IDateSpan),
             typeof(DateSpanPanel),
             new PropertyMetadata()
         );
        //TODO убрать после того как панель будет использоваться только в MVVM
        public static DependencyProperty CanBeZeroProperty = DependencyProperty.Register
         (
             "CanBeZero",
             typeof(bool),
             typeof(DateSpanPanel),
             new FrameworkPropertyMetadata(false)
         );
        //TODO убрать после того как панель будет использоваться только в MVVM
        public static DependencyProperty CanBeNullProperty = DependencyProperty.Register
         (
             "CanBeNull",
             typeof(bool),
             typeof(DateSpanPanel),
             new FrameworkPropertyMetadata(false)
         );

        public static DependencyProperty ShowClearButtonProperty = DependencyProperty.Register
         (
             "ShowClearButton",
             typeof(bool),
             typeof(DateSpanPanel),
             new FrameworkPropertyMetadata(false)
         );
        #endregion

        #region Properties
        //TODO убрать после того как панель будет использоваться только в MVVM
        public IDateSpan DateSpan
        {
            get { return (IDateSpan)GetValue(DateSpanProperty); }
            set { SetValue(DateSpanProperty, value); }
        }

        //TODO убрать после того как панель будет использоваться только в MVVM
        public bool CanBeZero
        {
            get { return (bool)GetValue(CanBeZeroProperty); }
            set { SetValue(CanBeZeroProperty, value); }
        }

        //TODO убрать после того как панель будет использоваться только в MVVM
        public bool CanBeNull
        {
            get { return (bool)GetValue(CanBeNullProperty); }
            set { SetValue(CanBeNullProperty, value); }
        }

        public bool ShowClearButton
        {
            get { return (bool)GetValue(ShowClearButtonProperty); }
            set { SetValue(ShowClearButtonProperty, value); }
        }
        #endregion

        public bool HasErrors()
        {
            bool hasError = false;
            if (this.Visibility == System.Windows.Visibility.Visible)
            {
                hasError = ValidateHelper.HasErrors(errorBorder, false);

                if (!hasError)                    
                    hasError = ((DateSpanViewModel)this.DataContext).HasErrors();
            }
            return hasError;
        }

        public DateSpanPanel()
        {
            InitializeComponent();
        }

        public static bool HasErrorsInSpecialUnitPanel(DependencyObject dp)
        {
            DateSpanPanel panel = (DateSpanPanel)dp;
            return panel.HasErrors();
        }

        //TODO после того как панель будет использоваться только в MVVM, убрать этот обработчик
        private void DateSpanP_Loaded(object sender, RoutedEventArgs e)
        {
            if (DateSpan != null)
                this.DataContext = new DateSpanViewModel(DateSpan, CanBeZero, CanBeNull);
        }
    }
}

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
using DBServices;

namespace Shared.SpecialUnit
{
    /// <summary>
    /// Логика взаимодействия для SpecialUnitPanelOld.xaml
    /// ========! Устарела. При поддержке паттерна MVVM использовать её новый вариант SpecialUnitPanel.xaml !============
    /// Панель используется для ввода комплексных (составных) чисел, которые состоят из нескольких целых чисел, - в этом случае должно быть задано свойство UnitType
    /// или для ввода одного вещественного числа - в этом случае должно быть задано свойство OtherUnitName .
    /// В PMS это: 
    /// 1) значение периода работы, тип счетчика которого может быть специальным (например, часы-минуты) - составное число 
    /// или пользовательским - одно вещетвенное число
    /// 2) показания счетчика, тип которого так же может быть специальным или пользовательским
    /// </summary>
    public partial class SpecialUnitPanelOld : UserControl
    {
        #region DependencyProperties
        public static DependencyProperty UnitTypeProperty = DependencyProperty.Register
        (
            "UnitType",
            typeof(SpecialUnitValue?),
            typeof(SpecialUnitPanelOld),
            new PropertyMetadata(OnUnitListTypeChanged)
        );

        public static DependencyProperty CurrentValueProperty = DependencyProperty.Register
        (
            "CurrentValue",
            typeof(decimal?),
            typeof(SpecialUnitPanelOld),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentValueChanged)
        );

        public static DependencyProperty OtherUnitNameProperty = DependencyProperty.Register
       (
           "OtherUnitName",
           typeof(string),
           typeof(SpecialUnitPanelOld),
           new FrameworkPropertyMetadata(OnOtherUnitNameChanged)
       );

        public static DependencyProperty CanBeNullProperty = DependencyProperty.Register
         (
             "CanBeNull",
             typeof(bool),
             typeof(SpecialUnitPanelOld),
             new FrameworkPropertyMetadata(false)
         );

        public static DependencyProperty CanBeZeroProperty = DependencyProperty.Register
         (
             "CanBeZero",
             typeof(bool),
             typeof(SpecialUnitPanelOld),
             new FrameworkPropertyMetadata(false)
         );

        public static DependencyProperty ShowClearButtonProperty = DependencyProperty.Register
        (
            "ShowClearButton",
            typeof(bool),
            typeof(SpecialUnitPanelOld),
            new FrameworkPropertyMetadata(false)
        );        
        #endregion

        #region Properties
        public SpecialUnitViewModelOld.ValidateMethod AdditionalValidate { get; set; }

        public SpecialUnitValue? UnitType
        {
            get { return (SpecialUnitValue?)GetValue(UnitTypeProperty); }
            set { SetValue(UnitTypeProperty, value); }
        }

        public decimal? CurrentValue
        {
            get { return (decimal?)GetValue(CurrentValueProperty); }
            set { SetValue(CurrentValueProperty, value); }
        }

        public string OtherUnitName
        {
            get { return (string)GetValue(OtherUnitNameProperty); }
            set { SetValue(OtherUnitNameProperty, value); }
        }

        public bool CanBeNull
        {
            get { return (bool)GetValue(CanBeNullProperty); }
            set { SetValue(CanBeNullProperty, value); }
        }

        public bool CanBeZero
        {
            get { return (bool)GetValue(CanBeZeroProperty); }
            set { SetValue(CanBeZeroProperty, value); }
        }

        public bool ShowClearButton
        {
            get { return (bool)GetValue(ShowClearButtonProperty); }
            set { SetValue(ShowClearButtonProperty, value); }
        }
        #endregion

        #region Validation

        public bool HasErrors()
        {
            bool hasError = false;
            if (this.Visibility == System.Windows.Visibility.Visible)
            {
                if (!UnitType.HasValue)
                    hasError = ValidateHelper.HasErrors(errorBorder, false);
                else
                {
                    string panelName = "panelForInput";

                    foreach (var item in list.Items)
                    {
                        ListBoxItem myListBoxItem = (ListBoxItem)(list.ItemContainerGenerator.ContainerFromItem(item));
                        hasError |= ValidateHelper.HasErrorsInTargetElement<Grid>(myListBoxItem, panelName, true);
                    }
                }

                if (!hasError)
                    hasError = ((SpecialUnitViewModelOld)this.DataContext).HasErrors();
            }
            return hasError;
        }
        #endregion

        public SpecialUnitPanelOld()
        {
            InitializeComponent();
        }

        private static void OnCurrentValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (((SpecialUnitPanelOld)obj).DataContext != null && ((SpecialUnitPanelOld)obj).DataContext is SpecialUnitViewModelOld)
            {
                ((SpecialUnitViewModelOld)(((SpecialUnitPanelOld)obj).DataContext)).ComplexNumberForEdit.BaseValue = (decimal?)e.NewValue;
            }
        }

        private static void OnUnitListTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (((SpecialUnitPanelOld)obj).DataContext != null && ((SpecialUnitPanelOld)obj).DataContext is SpecialUnitViewModelOld)
            {
                ((SpecialUnitViewModelOld)(((SpecialUnitPanelOld)obj).DataContext)).ComplexNumberForEdit.SetSpecialUnitType((SpecialUnitValue?)e.NewValue);
            }
        }

        private static void OnOtherUnitNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (((SpecialUnitPanelOld)obj).DataContext != null && ((SpecialUnitPanelOld)obj).DataContext is SpecialUnitViewModelOld)
            {
                ((SpecialUnitViewModelOld)(((SpecialUnitPanelOld)obj).DataContext)).ComplexNumberForEdit.SetOtherUnitName(e.NewValue != null ? e.NewValue.ToString() : null);
            }
        }

        private void OnBaseValueChanged(decimal? newValue)
        {
            SetValue(CurrentValueProperty, newValue);
        }

        public static bool HasErrorsInSpecialUnitPanel(DependencyObject dp)
        {
            Shared.SpecialUnit.SpecialUnitPanelOld panel = (Shared.SpecialUnit.SpecialUnitPanelOld)dp;
            return panel.HasErrors();
        }

        private void SpecialUnitP_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = new SpecialUnitViewModelOld(UnitType, CurrentValue, OtherUnitName, CanBeZero, CanBeNull, OnBaseValueChanged);
            if(AdditionalValidate != null)
                vm.AdditionalValidate += new SpecialUnitViewModelOld.ValidateMethod(AdditionalValidate);
            this.DataContext = vm;
        }

        
    }
}

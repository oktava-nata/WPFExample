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
    /// Логика взаимодействия для SpecialUnitPanel.xaml
    /// Панель используется для ввода комплексных (составных) чисел, которые состоят из нескольких целых чисел, или для ввода одного вещественного числа.
    /// В PMS это: 
    /// 1) значение периода работы, тип счетчика которого может быть специальным (например, часы-минуты) - составное число 
    /// или пользовательским - одно вещетвенное число
    /// 2) показания счетчика, тип которого так же может быть специальным или пользовательским
    /// 
    /// Данная панель является обновленной версией панели SpecialUnitPanelOld - она более подходит для использования в паттерне MVVM.
    /// 
    /// Для использования для данной панели необходимо задать свойство DataContext, в качестве значения должен выступать объект класса ComplexNumber.
    /// </summary>
    public partial class SpecialUnitPanel : UserControl
    {
        #region DependencyProperties
        public static DependencyProperty ShowClearButtonProperty = DependencyProperty.Register
        (
            "ShowClearButton",
            typeof(bool),
            typeof(SpecialUnitPanel),
            new FrameworkPropertyMetadata(false)
        );        
        public bool ShowClearButton
        {
            get { return (bool)GetValue(ShowClearButtonProperty); }
            set { SetValue(ShowClearButtonProperty, value); }
        }
        #endregion

        public SpecialUnitPanel()
        {
            InitializeComponent();
        }
    }
}

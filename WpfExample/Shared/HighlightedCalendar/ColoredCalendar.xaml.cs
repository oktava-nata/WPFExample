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
using System.Collections.ObjectModel;

namespace Shared.HighlightedCalendar
{
    /// <summary>
    /// Логика взаимодействия для ColoredCalendar.xaml
    /// </summary>
    public partial class ColoredCalendar : UserControl
    {
        #region Delegates
        public delegate List<IColoredDay> VisiblePeriodChangedHandler(DateTime dateFrom, DateTime dateTo);
        public event VisiblePeriodChangedHandler OnVisiblePeriodChanged;

        public delegate bool BeforeNewSelectingHandler();
        public event BeforeNewSelectingHandler OnBeforeNewSelecting;

        public delegate bool AfterNewSelectingHandler();
        public event AfterNewSelectingHandler OnAfterNewSelecting;
        #endregion

        #region Dependency Properties
        public static DependencyProperty VisibleDatesProperty = DependencyProperty.Register
        (
            "VisibleDates",
            typeof(ObservableCollection<VisibleDay>),
            typeof(ColoredCalendar),
            new PropertyMetadata()
        );

        public static DependencyProperty SelectedDatesProperty = DependencyProperty.Register
        (
            "SelectedDates",
            typeof(List<IColoredDay>),
            typeof(ColoredCalendar),
            new PropertyMetadata()
        );

        public static DependencyProperty SelectedDateProperty = DependencyProperty.Register
        (
            "SelectedDate",
            typeof(IColoredDay),
            typeof(ColoredCalendar),
            new PropertyMetadata()
        );

        public static DependencyProperty AutoSelectTodayProperty = DependencyProperty.Register
        (
            "AutoSelectToday",
            typeof(bool),
            typeof(ColoredCalendar),
            new PropertyMetadata()
        );

        public static DependencyProperty MultiSelectProperty = DependencyProperty.Register
        (
            "MultiSelect",
            typeof(bool),
            typeof(ColoredCalendar),
            new PropertyMetadata()
        );

        public static DependencyProperty VisibleMonthProperty = DependencyProperty.Register
        (
            "VisibleMonth",
            typeof(int),
            typeof(ColoredCalendar),
            new PropertyMetadata()
        );

        public static DependencyProperty VisibleYearProperty = DependencyProperty.Register
        (
            "VisibleYear",
            typeof(int),
            typeof(ColoredCalendar),
            new PropertyMetadata()
        );

        public static DependencyProperty SelectionBrushProperty = DependencyProperty.Register
        (
            "SelectionBrush",
            typeof(Brush),
            typeof(ColoredCalendar),
            new PropertyMetadata()
        );

        public static DependencyProperty CanSelectProperty = DependencyProperty.Register
        (
            "CanSelect",
            typeof(bool),
            typeof(ColoredCalendar),
            new PropertyMetadata()
        );

        public static DependencyProperty SelectionShapeLikeColoringProperty = DependencyProperty.Register
        (
            "SelectionShapeLikeColoring",
            typeof(bool),
            typeof(ColoredCalendar),
            new PropertyMetadata()
        );

        public static DependencyProperty AlwaysExecuteAfterNewSelectingEventProperty = DependencyProperty.Register
        (
            "AlwaysExecuteAfterNewSelectingEvent",
            typeof(bool),
            typeof(ColoredCalendar),
            new PropertyMetadata(false)
        );
        #endregion

        public ObservableCollection<VisibleDay> VisibleDates
        {
            get { return (ObservableCollection<VisibleDay>)GetValue(VisibleDatesProperty); }
            private set { SetValue(VisibleDatesProperty, value); }
        }

        public List<IColoredDay> SelectedDates
        {
            get { return (List<IColoredDay>)GetValue(SelectedDatesProperty); }
            private set { SetValue(SelectedDatesProperty, value); }
        }

        public IColoredDay SelectedDate
        {
            get { return (IColoredDay)GetValue(SelectedDateProperty); }
            private set { SetValue(SelectedDateProperty, value); }
        }

        public bool AutoSelectToday
        {
            get { return (bool)GetValue(AutoSelectTodayProperty); }
            set { SetValue(AutoSelectTodayProperty, value); }
        }

        public bool MultiSelect
        {
            get { return (bool)GetValue(MultiSelectProperty); }
            set { SetValue(MultiSelectProperty, value); }
        }

        public int VisibleMonth
        {
            get { return (int)GetValue(VisibleMonthProperty); }
            private set { SetValue(VisibleMonthProperty, value); }
        }

        public int VisibleYear
        {
            get { return (int)GetValue(VisibleYearProperty); }
            private set { SetValue(VisibleYearProperty, value); }
        }

        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        public bool CanSelect
        {
            get { return (bool)GetValue(CanSelectProperty); }
            set { SetValue(CanSelectProperty, value); }
        }

        /// <summary>
        /// Визуально форма выделения совпадает с формой раскрашиваемой подложки дня
        /// </summary>
        public bool SelectionShapeLikeColoring
        {
            get { return (bool)GetValue(SelectionShapeLikeColoringProperty); }
            set { SetValue(SelectionShapeLikeColoringProperty, value); }
        }

        /// <summary>
        /// Всегда выполнять метод OnAfterNewSelecting, 
        /// даже если выделение не было изменено (новое выделение совпало со старым)
        /// </summary>
        public bool AlwaysExecuteAfterNewSelectingEvent
        {
            get { return (bool)GetValue(AlwaysExecuteAfterNewSelectingEventProperty); }
            set { SetValue(AlwaysExecuteAfterNewSelectingEventProperty, value); }
        }
        
        #region Constructor
        public ColoredCalendar()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var today = DateTime.Today;
            SetDisplayMonthAndYear(today.Year, today.Month);
            if (AutoSelectToday)
                SetSelectedToday();
        } 
        #endregion

        public void UnselectAll()
        {
            ClearSelection(false);
        }

        #region Btn_Click
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            var date = new DateTime(VisibleYear, VisibleMonth, 1);
            var newDate = date.AddMonths(1);
            SetDisplayMonthAndYear(newDate.Year, newDate.Month);
        }

        private void BtnLast_Click(object sender, RoutedEventArgs e)
        {
            var date = new DateTime(VisibleYear, VisibleMonth, 1);
            var newDate = date.AddMonths(-1);
            SetDisplayMonthAndYear(newDate.Year, newDate.Month);
        }
        #endregion

        #region Обработка событий выделения
        private void VisibleDay_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem day = (ListBoxItem)sender;
            VisibleDay visibleDay = (VisibleDay)day.Content;

            if (!visibleDay.IsAdditionalDay && CanSelect)
            {
                //выполняем действия для текущего (старого) выделения
                if (!ProcessAndClearSelection(false))
                    return;

                visibleDay.Selected = true;

                if (MultiSelect)
                {
                    DragDrop.DoDragDrop(day, visibleDay, DragDropEffects.Copy);
                }
                Mouse.Capture(day);
            }
        }

        private void VisibleDay_DragOver(object sender, DragEventArgs e)
        {
            var targetDay = (VisibleDay)((ListBoxItem)sender).Content;
            //если это день, а не пустая клетка
            if (!targetDay.IsAdditionalDay)
            {
                //стираем выделение т.к. пользователь меняет его на ходу перемещая курсор
                foreach (var item in VisibleDates)
                {
                    item.Selected = false;
                }

                var sourceDay = (VisibleDay)e.Data.GetData(typeof(VisibleDay));

                int targetIndex = VisibleDates.IndexOf(targetDay);
                int sourceIndex = VisibleDates.IndexOf(sourceDay);

                int min = Math.Min(targetIndex, sourceIndex);
                int max = Math.Max(targetIndex, sourceIndex);
                for (int i = 0; i < VisibleDates.Count; i++)
                {
                    if (i >= min && i <= max)
                        VisibleDates[i].Selected = true;
                }
            }

        }

        private void VisibleDay_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if (UpdateSelectedDates() || AlwaysExecuteAfterNewSelectingEvent)
            {
                if (OnAfterNewSelecting != null)
                    OnAfterNewSelecting();
            }
        }
        #endregion
    }
}

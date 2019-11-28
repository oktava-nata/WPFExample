using System;
using System.Windows;
using System.Windows.Controls;
using BaseLib.AdditionalTypes;

namespace Shared.Forms.Panels
{
    /// <summary>
    /// Логика взаимодействия для DatePeriodPanel.xaml
    /// </summary>
    public partial class DatePeriodPanel : UserControl
    {
        public static DependencyProperty PeriodProperty;
        public Period Period
        {
            get { return (Period)GetValue(PeriodProperty); }
            set { SetValue(PeriodProperty, value); }
        }

        public static DependencyProperty MaxPeriodInMonthProperty;
        public int? MaxPeriodInMonth
        {
            get { return (int?)GetValue(MaxPeriodInMonthProperty); }
            set { SetValue(MaxPeriodInMonthProperty, value); }
        }

        public static DependencyProperty DateFromIsNullableProperty;
        public bool DateFromIsNullable
        {
            get { return (bool)GetValue(DateFromIsNullableProperty); }
            set { SetValue(DateFromIsNullableProperty, value); }
        }

        public static DependencyProperty DateToIsNullableProperty;
        public bool DateToIsNullable
        {
            get { return (bool)GetValue(DateToIsNullableProperty); }
            set { SetValue(DateToIsNullableProperty, value); }
        }

        public static DependencyProperty DateFromProperty;
        public DateTime? DateFrom
        {
            get { return (DateTime?)GetValue(DateFromProperty); }
            set { SetValue(DateFromProperty, value); }
        }

        public static DependencyProperty DateToProperty;
        public DateTime? DateTo
        {
            get { return (DateTime?)GetValue(DateToProperty); }
            set { SetValue(DateToProperty, value); }
        }

        public static DependencyProperty ShowLblFromProperty;
        public bool ShowLblFrom
        {
            get { return (bool)GetValue(ShowLblFromProperty); }
            set { SetValue(ShowLblFromProperty, value); }
        }

        #region Constructors
        public DatePeriodPanel()
        {
            InitializeComponent();
            Period = new Period();
            Period.PeriodChanged += new Period.PeriodChangedHandler(ClearDisabledDatesForMaxPeriod);
            Period.AfterPeriodChanged += new Period.AfterPeriodChangedHandler(SetDisabledDatesForMaxPeriod);
            Period.DateFromChanged += new Period.DateChangedHandler(BackDateFrom_OnChanged);
            Period.DateToChanged += new Period.DateChangedHandler(BackDateTo_OnChanged);
        }

        static DatePeriodPanel()
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata(new PropertyChangedCallback(Period_OnChanged));
            PeriodProperty = DependencyProperty.Register("Period", typeof(Period),
                typeof(DatePeriodPanel), metadata);

            FrameworkPropertyMetadata metadataMaxPeriod = new FrameworkPropertyMetadata(null);
            MaxPeriodInMonthProperty = DependencyProperty.Register("MaxPeriodInMonth", typeof(int?),
                typeof(DatePeriodPanel), metadataMaxPeriod);

            FrameworkPropertyMetadata metadataDateFromIsNullable = new FrameworkPropertyMetadata(false, new PropertyChangedCallback(IsNulable_OnChanged));
            DateFromIsNullableProperty = DependencyProperty.Register("DateFromIsNullable", typeof(bool),
                typeof(DatePeriodPanel), metadataDateFromIsNullable);

            FrameworkPropertyMetadata metadataDateToIsNullable = new FrameworkPropertyMetadata(false, new PropertyChangedCallback(IsNulable_OnChanged));
            DateToIsNullableProperty = DependencyProperty.Register("DateToIsNullable", typeof(bool),
                typeof(DatePeriodPanel), metadataDateToIsNullable);

            FrameworkPropertyMetadata metadataDateFrom = new FrameworkPropertyMetadata(DateTime.MinValue, new PropertyChangedCallback(DateFrom_OnChanged));
            DateFromProperty = DependencyProperty.Register("DateFrom", typeof(DateTime?),
                typeof(DatePeriodPanel), metadataDateFrom);

            FrameworkPropertyMetadata metadataDateTo = new FrameworkPropertyMetadata(DateTime.MaxValue, new PropertyChangedCallback(DateTo_OnChanged));
            DateToProperty = DependencyProperty.Register("DateTo", typeof(DateTime?),
                typeof(DatePeriodPanel), metadataDateTo);

            ShowLblFromProperty = DependencyProperty.Register("ShowLblFrom", typeof(bool), typeof(DatePeriodPanel), new FrameworkPropertyMetadata(true));
        }

        #endregion

        private void dateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //отлавливаем ввод пустого значения если дата не может быть нулл
            if (!DateFromIsNullable && !dp_dateFrom.SelectedDate.HasValue && Period != null)
                dp_dateFrom.SelectedDate = Period.DateFrom;
        }

        private void dateTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //отлавливаем ввод пустого значения  если дата не может быть нулл
            if (!DateToIsNullable && !dp_dateTo.SelectedDate.HasValue && Period != null)
                dp_dateTo.SelectedDate = Period.DateTo;
        }

        void SetDisabledDatesForMaxPeriod(DateTime? startDate, DateTime? endDate)
        {
            dp_dateFrom.BlackoutDates.Clear();
            dp_dateTo.BlackoutDates.Clear();

            //устанавливаем BlackoutDates если период ограничен
            if (!DateFromIsNullable && !DateToIsNullable && MaxPeriodInMonth.HasValue)
            {
                //проверяем на всякий случай, хотя предполагаем, что логика следит за тем, что значение не нулл
                if (endDate.HasValue)
                {
                    //в календаре dp_dateFrom новая дата еще не установлена
                    //нужно ее установить, до того как навесить новое ограничение по датам, иначе будет вылетать (при вводе с клавы!)
                    if (!dp_dateFrom.SelectedDate.HasValue || dp_dateFrom.SelectedDate < startDate)
                        dp_dateFrom.SelectedDate = startDate;
                    dp_dateFrom.BlackoutDates.Add(new CalendarDateRange { End = endDate.Value.AddMonths(0 - MaxPeriodInMonth.Value).Date });
                }
                //проверяем на всякий случай, хотя предполагаем, что логика следит за тем, что значение не нулл (при вводе с клавы!)
                if (startDate.HasValue)
                {
                    //в календаре dp_dateTo новая дата еще не установлена
                    //нужно ее установить, до того как навесить новое ограничение по датам, иначе будет вылетать
                    if (!dp_dateTo.SelectedDate.HasValue || dp_dateTo.SelectedDate > endDate)
                        dp_dateTo.SelectedDate = endDate;
                    dp_dateTo.BlackoutDates.Add(new CalendarDateRange { Start = startDate.Value.AddMonths(MaxPeriodInMonth.Value).Date });
                }
            }
        }

        void ClearDisabledDatesForMaxPeriod(DateTime? startDate, DateTime? endDate)
        {
            dp_dateFrom.BlackoutDates.Clear();
            dp_dateTo.BlackoutDates.Clear();
        }

        private void DatePeriodPan_Loaded(object sender, RoutedEventArgs e)
        {
            if (Period != null) Period.OnAfterPeriodChanged();
        }

        private void datePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            //что бы при удалении даты, когда IsNulable = false
            var picker = (DatePicker)sender;
            if (picker.SelectedDate != null)
                picker.Text = picker.SelectedDate.Value.ToShortDateString();
        }

        //добавлен для того чтобы работал Binding к свойству Period TwoWay
        static void Period_OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var period = (obj as DatePeriodPanel).Period;
            if (period == null) return;

            period.PeriodChanged += new Period.PeriodChangedHandler((obj as DatePeriodPanel).ClearDisabledDatesForMaxPeriod);
            period.AfterPeriodChanged += new Period.AfterPeriodChangedHandler((obj as DatePeriodPanel).SetDisabledDatesForMaxPeriod);
            period.DateFromChanged += new Period.DateChangedHandler((obj as DatePeriodPanel).BackDateFrom_OnChanged);
            period.DateToChanged += new Period.DateChangedHandler((obj as DatePeriodPanel).BackDateTo_OnChanged);
        }

        static void IsNulable_OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var panel = (DatePeriodPanel)obj;

            panel.Period.DateFromIsNullable = panel.DateFromIsNullable;
            panel.Period.DateToIsNullable = panel.DateToIsNullable;
        }

        static void DateFrom_OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var panel = (DatePeriodPanel)obj;
            panel.Period.DateFrom = panel.DateFrom;
        }

        static void DateTo_OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var panel = (DatePeriodPanel)obj;
            panel.Period.DateTo = panel.DateTo;
        }

        void BackDateFrom_OnChanged()
        {
            SetValue(DateFromProperty, Period.DateFrom);
        }

        void BackDateTo_OnChanged()
        {
            SetValue(DateToProperty, Period.DateTo);
        }

        public void SetFocusOnDateFrom()
        {
            dp_dateFrom.Focus();
        }
    }
}

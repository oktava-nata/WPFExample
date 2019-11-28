using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Shared.HighlightedCalendar
{
    public partial class ColoredCalendar
    {
        private void SetDisplayMonthAndYear(int year, int month)
        {
            if (ProcessAndClearSelection(true))
            {
                VisibleMonth = month;
                VisibleYear = year;
                GetVisibleDays();
            }
        }

        /// <summary>
        /// Обработка выделенных данных и очистка выделения
        /// </summary>
        /// <param name="updateSelectedDates">Обновить свойство SelectedDates и SelectedDate</param>
        /// <returns></returns>
        private bool ProcessAndClearSelection(bool updateSelectedDates)
        {
            //выполняем действия для текущего (старого) выделения
            if (SelectedDates != null && SelectedDates.Count() > 0 && OnBeforeNewSelecting != null)
                //если неудача, то оставляем старое выделение
                if (!OnBeforeNewSelecting())
                    return false;

            ClearSelection(updateSelectedDates);
            return true;
        }

        private void ClearSelection(bool updateSelectedDates)
        {
            if (VisibleDates != null)
            {
                //стираем старое выделение
                foreach (var item in VisibleDates)
                {
                    item.Selected = false;
                }

                if (updateSelectedDates)
                    UpdateSelectedDates();
            }
        }

        /// <summary>
        /// Устанавливает выделение на сегодняшний день. Применяется только при Load панели
        /// </summary>
        private void SetSelectedToday()
        {
            if (VisibleDates != null)
            {
                var today = VisibleDates.Where(i => i.ColoredDay != null && i.ColoredDay.Date == DateTime.Today).FirstOrDefault();
                if (today != null)
                    today.Selected = true;
                UpdateSelectedDates();
            }
        }

        private bool UpdateSelectedDates()
        {
            var newSelection = VisibleDates.Where(i => i.Selected).Select(i => i.ColoredDay).ToList();
            if (newSelection.Count == 0 && (SelectedDates == null || SelectedDates.Count == 0))
                return false;

            if (SelectedDates != null)
            {     
                if (newSelection.Count == SelectedDates.Count && newSelection[0].Date == SelectedDates[0].Date)
                    return false;
            }
            SelectedDates = newSelection;
            SelectedDate = SelectedDates.Count() > 0 ? SelectedDates[0] : null;
            return true;
        }

        #region Get VisibleDays
        private void GetVisibleDays()
        {
            if (OnVisiblePeriodChanged != null)
            {
                DateTime firstDateInMonth = new DateTime(VisibleYear, VisibleMonth, 1);
                DateTime lastDateInMonth = new DateTime(VisibleYear, VisibleMonth, DateTime.DaysInMonth(VisibleYear, VisibleMonth));

                var realDates = OnVisiblePeriodChanged(firstDateInMonth, lastDateInMonth);

                var list = new ObservableCollection<VisibleDay>();

                int firstEmptyDays = GetCountOfFirstEmptyDays(firstDateInMonth);
                for (int i = 0; i < firstEmptyDays; i++)
                {
                    list.Add(new VisibleDay());
                }

                if (realDates != null)
                {
                    var checkedDate = firstDateInMonth;
                    while (checkedDate <= lastDateInMonth)
                    {
                        var realDate = realDates.Where(item => item.Date == checkedDate).FirstOrDefault();
                        if (realDate == null)
                            list.Add(new VisibleDay(new SimpleColoredDay(checkedDate)));
                        else
                            list.Add(new VisibleDay(realDate));
                        checkedDate = checkedDate.AddDays(1);
                    }
                }

                int lastEmptyDays = GetCountOfLastEmptyDays(lastDateInMonth);
                for (int i = 0; i < lastEmptyDays; i++)
                {
                    list.Add(new VisibleDay());
                }

                VisibleDates = list;
            }
        }

        private static int GetCountOfFirstEmptyDays(DateTime firstDateInMonth)
        {
            switch (firstDateInMonth.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                default:
                    return 0;
            }
        }

        private static int GetCountOfLastEmptyDays(DateTime lastDateInMonth)
        {
            switch (lastDateInMonth.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    return 2;
                case DayOfWeek.Monday:
                    return 6;
                case DayOfWeek.Saturday:
                    return 1;
                case DayOfWeek.Sunday:
                    return 0;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Tuesday:
                    return 5;
                case DayOfWeek.Wednesday:
                    return 4;
                default:
                    return 0;
            }
        }
        #endregion
    }
}

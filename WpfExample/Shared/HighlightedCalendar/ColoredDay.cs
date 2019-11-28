using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;

namespace Shared.HighlightedCalendar
{
    public class VisibleDay : BaseLib.Services.NotifyPropertyChanged
    {
        public IColoredDay ColoredDay { get; private set; }
        public bool IsAdditionalDay { get; private set; }

        public VisibleDay() { IsAdditionalDay = true; }
        public VisibleDay(IColoredDay entity)
        {
            ColoredDay = entity;
        }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                RaisePropertyChanged(() => this.Selected);
            }
        }
    }

    public interface IColoredDay
    {
        DateTime Date { get; set; }
        SolidColorBrush Brush { get; }
        string ToolTip { get; }
    }

    public class SimpleColoredDay : IColoredDay
    {
        public DateTime Date { get; set; }
        public SolidColorBrush Brush { get { return new SolidColorBrush(Colors.Transparent); } }
        public string ToolTip { get { return null; } }

        public SimpleColoredDay(DateTime date)
        {
            Date = date;
        }
    }

}

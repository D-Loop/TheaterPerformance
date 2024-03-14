using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.Calendar;

namespace KPO.Helper
{
    public class DayButtonTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate DisabledTemplate { get; set; }

        public DataTemplate BookedDayTemplate { get; set; }
        public DataTemplate SpecialHolidayTemplate { get; set; }

        public List<DateTime> BookedDays { get; set; }
        public List<DateTime> SpecialHolidays { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var calendarButton = item as CalendarButtonContent;

            return this.DefaultTemplate;
        }
    }
}
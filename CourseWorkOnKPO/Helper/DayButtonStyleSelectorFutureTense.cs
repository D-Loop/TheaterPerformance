using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.Calendar;

namespace KPO.Helper
{
    class DayButtonStyleSelectorFutureTense : StyleSelector
    {

        const int TimeBlockToday=22; //Часы блокировки сегодняшней даты в каллендаре

        public Style SpecialStyleWorkingDay { get; set; }
        public Style SpecialStyleHoliday { get; set; }

        public static bool ModeNewScheme { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            CalendarButtonContent content = item as CalendarButtonContent;

            if (content != null)
            {
                if (content.ButtonType == CalendarButtonType.Date)
                {
                    if (content.Date.AddHours(TimeBlockToday) <DateTime.Now) content.IsEnabled = false;
                    if (content.Date.DayOfWeek == DayOfWeek.Saturday || content.Date.DayOfWeek == DayOfWeek.Sunday) return SpecialStyleHoliday;
                    return SpecialStyleWorkingDay;
                }
            }
            return base.SelectStyle(item, container);
        }
    }
}

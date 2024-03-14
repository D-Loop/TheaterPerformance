using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using static System.String;

namespace KPO.DataModels{
	public class TheaterPerformance  {

        public int Index { get; set; }
        public string Name { get; set; }
        public string PreviewSource => AppDomain.CurrentDomain.BaseDirectory + "Helper\\DATA\\Preview\\" + Name + ".jpg";
        public DateTime PlanTime { get; set; }
        public int Room { get; set; }
        public string  Description { get; set; }
        public string  Persons { get; set; }
        public string  Creators { get; set; }
        public string Duration { get; set; }
        public int Cost { get; set; }

    }
}

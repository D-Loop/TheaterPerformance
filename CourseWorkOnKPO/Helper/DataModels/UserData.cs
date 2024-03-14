using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using static System.String;

namespace KPO.DataModels {
	public class UserData{

        public string Surname { get; set; }
        public string Name { get; set; }
        public int CVV { get; set; }
        public string CartNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Date { get; set; }
       
    }
}

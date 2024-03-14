using KPO.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using static System.String;

namespace KPO.VM {
	public class UserDataVM : UserData, INotifyPropertyChanged, ICloneable {

        #region OnPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "") {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
		#endregion

        public UserDataVM()
        {

        }

        public new string Surname
        {
            get => base.Surname;
            set
            {
                base.Surname = value;
                OnPropertyChanged("Surname");
            }
        }
        public new string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public new int CVV
        {
            get => base.CVV;
            set
            {
                base.CVV = value;
                OnPropertyChanged("CVV");
            }
        }
        public new string CartNumber
        {
            get => base.CartNumber;
            set
            {
                base.CartNumber = value;
                OnPropertyChanged("CartNumber");
            }
        }
        public new string Email
        {
            get => base.Email;
            set
            {
                base.Email = value;
                OnPropertyChanged("Email");
            }
        }
        public new string PhoneNumber
        {
            get => base.PhoneNumber;
            set
            {
                base.PhoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }
        public new string Date
        {
            get => base.Date;
            set
            {
                base.Date = value;
                OnPropertyChanged("Date");
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

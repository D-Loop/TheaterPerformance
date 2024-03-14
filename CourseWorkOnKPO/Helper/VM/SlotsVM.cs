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

namespace KPO.VM
{
	public class SlotsVM : Slots, INotifyPropertyChanged, ICloneable {

        #region OnPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "") {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
		#endregion

        public SlotsVM()
        {

        }

        public new bool IsFree
        {
            get => base.IsFree;
            set
            {
                base.IsFree = value;
                OnPropertyChanged("IsFree");
            }
        }
        public new int Index
        {
            get => base.Index;
            set
            {
                base.Index = value;
                OnPropertyChanged("Index");
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

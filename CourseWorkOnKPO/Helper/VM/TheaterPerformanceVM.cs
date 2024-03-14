using KPO.DataModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KPO.VM
{
	public class TheaterPerformanceVM : TheaterPerformance, INotifyPropertyChanged, ICloneable {

        #region OnPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "") {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
		#endregion

        public TheaterPerformanceVM()
        {

        }

        public new int Index
        {
            get =>  base.Index;
            set
            {
                base.Index = value;
                OnPropertyChanged("Index");
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
        
        public string PlanTimeVM => base.PlanTime == DateTime.MinValue ? "- - -" : base.PlanTime.ToString("dd.MM.yyyy HH:mm");
        public string CostVM => Cost == 0 ? "- - -" : Cost.ToString();
        public new int Room
        {
            get => base.Room;
            set
            {
                base.Room = value;
                OnPropertyChanged("Room");
            }
        }
        public new string Description
        {
            get => base.Description;
            set
            {
                base.Description = value;
                OnPropertyChanged("Description");
            }
        }
        public new string Persons
        {
            get => base.Persons;
            set
            {
                base.Persons = value;
                OnPropertyChanged("Persons");
            }
        }
        public new string Creators
        {
            get => base.Creators;
            set
            {
                base.Creators = value;
                OnPropertyChanged("Creators");
            }
        }
        public new string Duration
        {
            get => base.Duration;
            set
            {
                base.Duration = value;
                OnPropertyChanged("Duration");
            }
        }
        public new int Cost
        {
            get => base.Cost;
            set
            {
                base.Cost = value;
                OnPropertyChanged("Cost");
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

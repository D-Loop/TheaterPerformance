using System.Collections.ObjectModel;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace KPO.Helper
{
    public class ModeltemMenu : ViewModelBase
    {
        string _Header;

        public string Header
        {
            get { return _Header; }
            set
            {
                _Header = value;
                this.OnPropertyChanged("IncludedProcessGroups");
            }

        }

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }


        private string _Icon;

        public string Icon
        {
            get { return _Icon; }
            set
            {
                this._Icon = value;
                this.OnPropertyChanged("Icon");
            }
        }


        private bool _IsEnabled;

        public bool IsEnabled
        {
            get { return !_IsEnabled; }
            set
            {
                _IsEnabled = value;
                this.OnPropertyChanged("IsEnabled");
            }

        }




        private bool _IsSeparator;

        public bool IsSeparator
        {
            get { return _IsSeparator; }
            set
            {
                _IsSeparator = value;
                this.OnPropertyChanged("IsSeparator");
            }

        }


        private ObservableCollection<ModeltemMenu> _Items;

        public ObservableCollection<ModeltemMenu> Items
        {
            get { return _Items; }
            set
            {
                _Items = value;
                this.OnPropertyChanged("Items");
            }
        }

        public ModeltemMenu()
        {
            Items = new ObservableCollection<ModeltemMenu>();
        }





    }

}

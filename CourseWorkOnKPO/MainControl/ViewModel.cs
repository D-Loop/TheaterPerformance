using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Office.Interop.Excel;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Calendar;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Persistence;
using Telerik.Windows.Controls.GridView.SearchPanel;
using Telerik.Windows.Controls.Map;
using Telerik.Windows.Data;
using Application = System.Windows.Application;
using KPO.VM;
using KPO.Helper;

namespace KPO.MainControl {
    public class ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        #region OnPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        public RadCalendar CalendarFrom;
        public RadCalendar CalendarTo;
        public RadGridView GridCartoonsList;

        #region CONSTRUCTORS

        public ViewModel()
        {
            //Смена режима копирования данных в гриде
            CommandCopying = new DelegateCommand(OnCopying);
            CommandClipboardMode = new DelegateCommand(OnClipboardMode);
            CommandCopyingCellClipboard = new DelegateCommand(OnCopyingCellClipboard);
            CommandUpdateData = new DelegateCommand(OnUpdateData);
            CommandClearFilterGrid = new DelegateCommand(OnClearFilterGrid);
            CommandMouseDoubleClickGrid = new DelegateCommand(OnMouseDoubleClickGrid);
            CommandCreateNewTheaterPerformance = new DelegateCommand(OnCreateNewTheaterPerformance);
            CommandBackToMain = new DelegateCommand(OnBackToMain);
            CommandSaveOrder = new DelegateCommand(OnSaveOrder);    

            // смена дат календарей
            CommandCalendarToToday = new DelegateCommand(OnCalendarToToday);
            CommandCalendarFromToday = new DelegateCommand(OnCalendarFromToday);

            // правая панель
            CommandToggleRightPanel = new DelegateCommand(OnToggleRightPanel);

            ActiveTab = 0;
            TwoTabVisibility = Visibility.Collapsed;
            VisibilityRightPanel = Visibility.Collapsed;

            SelectedDateTo = DateTime.Today;
            SelectedDateFrom = DateTime.Today.AddDays(-1);
            DisplayDateFrom = DateTime.Today.AddDays(-1);
            DisplayDateTo = DateTime.Today.AddDays(1);
            
            ListSlots = new Dictionary<int,SlotsVM>();

            for (var i = 1; i < 79; i++)
                ListSlots.Add(i,new SlotsVM(){Index = i, IsFree = true} );

        }

        #endregion

        #region TemplateProperty

        private SlotsVM _SelectedSlots;
        public SlotsVM SelectedSlots
        {
            get => _SelectedSlots ?? new SlotsVM();
            set
            {
                if(value != null && value.IsFree)
                    _SelectedSlots = value;

                OnPropertyChanged("SelectedSlots");
            }
        }


        private Dictionary<int,SlotsVM> _listSlots;

        public Dictionary<int,SlotsVM> ListSlots
        {
            get => _listSlots ?? new Dictionary<int,SlotsVM>();
            set
            {
                _listSlots = value;
                OnPropertyChanged("ListSlots");
            }
        }

        private ObservableCollection<TheaterPerformanceVM> _listTheaterPerformance;

        public ObservableCollection<TheaterPerformanceVM> ListTheaterPerformance
        {
            get => _listTheaterPerformance ?? new ObservableCollection<TheaterPerformanceVM>();
            set
            {
                _listTheaterPerformance = value;
                OnPropertyChanged("ListTheaterPerformance");
            }
        }
        
        private UserDataVM _newOrder;

        public UserDataVM NewOrder
        {
            get => _newOrder ?? new UserDataVM();
            set
            {
                _newOrder = value;
                OnPropertyChanged("NewOrder");
            }
        }
        private TheaterPerformanceVM _selectedCustomTheaterPerformance;

        public TheaterPerformanceVM SelectedCustomTheaterPerformance
        {
            get => _selectedCustomTheaterPerformance ?? new TheaterPerformanceVM();
            set
            {
                _selectedCustomTheaterPerformance = value;
                OnPropertyChanged("SelectedCustomTheaterPerformance");
            }
        }
        private ObservableCollection<TheaterPerformanceVM> _listCustomTheaterPerformance;

        public ObservableCollection<TheaterPerformanceVM> ListCustomTheaterPerformance
        {
            get => _listCustomTheaterPerformance ?? new ObservableCollection<TheaterPerformanceVM>();
            set
            {
                _listCustomTheaterPerformance = value;
                OnPropertyChanged("ListCustomTheaterPerformance");
            }
        }

        private int _activeTab;

        public int ActiveTab
        {
            get => _activeTab;
            set
            {
                _activeTab = value;

                TwoTabVisibility = _activeTab == 1 ? Visibility.Visible : Visibility.Collapsed;

                OnPropertyChanged("ActiveTab");
            }
        }


        private DateTime _displayDateFrom;

        public DateTime DisplayDateFrom
        {
            get => _displayDateFrom;
            set
            {
                _displayDateFrom = value;
                OnPropertyChanged("DisplayDateFrom");
            }
        }

        private DateTime _displayDateTo;

        public DateTime DisplayDateTo
        {
            get => _displayDateTo;
            set
            {
                _displayDateTo = value;
                OnPropertyChanged("DisplayDateTo");
            }
        }


        private DateTime _selectedDateFrom;

        /// <summary>
        /// Дата от для календаря использованых календарей
        /// </summary>
        public DateTime SelectedDateFrom
        {
            get => _selectedDateFrom;
            set
            {
                _selectedDateFrom = value;
                if (_selectedDateTo < _selectedDateFrom) SelectedDateTo = value;
                if ((_selectedDateTo - _selectedDateFrom).TotalDays > 31)
                    _selectedDateFrom = value;

                DisplayDateFrom = SelectedDateFrom;
                DisplayDateTo = SelectedDateTo;

                OnPropertyChanged("SelectedDateFrom");
            }
        }

        /// <summary> Дата до для использованых календарей </summary>
        private DateTime _selectedDateTo;

        public DateTime SelectedDateTo
        {
            get => _selectedDateTo;
            set
            {
                _selectedDateTo = value;
                if (_selectedDateTo < _selectedDateFrom) SelectedDateFrom = value;
                if ((_selectedDateTo - _selectedDateFrom).TotalDays > 31)
                    _selectedDateTo = value;

                DisplayDateFrom = SelectedDateFrom;
                DisplayDateTo = SelectedDateTo;

                OnPropertyChanged("SelectedDateTo");
            }
        }

        private Visibility _twoTabVisibility;

        public Visibility TwoTabVisibility
        {
            get { return _twoTabVisibility; }
            set
            {
                _twoTabVisibility = value;
                OnPropertyChanged("TwoTabVisibility");
            }
        }

        private Visibility _visibilityRightPanel;

        public Visibility VisibilityRightPanel
        {
            get { return _visibilityRightPanel; }
            set
            {
                _visibilityRightPanel = value;
                OnPropertyChanged("VisibilityRightPanel");
            }
        }

        private int _WidthRightPanel;

        public int WidthRightPanel
        {
            get
            {
                if (_WidthRightPanel == 0)
                    _WidthRightPanel = 550;
                return _WidthRightPanel;
            }
            set
            {
                _WidthRightPanel = value;
                OnPropertyChanged("WidthRightPanel");
            }
        }

        private int _selectedIndexRightPanel;

        public int SelectedIndexRightPanel
        {
            get { return _selectedIndexRightPanel; }
            set
            {
                //if (value == 1)
                //CommandGetSKUInSupply.Execute(null);//Получаем данные по текущей поставке

                _selectedIndexRightPanel = value;
                OnPropertyChanged("SelectedIndexRightPanel");
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }



        #endregion

        #region MessageTimerString

        private string _MessageString;

        public string MessageString
        {
            get => _MessageString;
            set
            {
                _MessageString = value;
                if (value != null)
                {
                    StartTimerMessageStringEntity(5);
                }

                OnPropertyChanged("MessageString");
            }
        }


        private DispatcherTimer _timerMessageStringEntity;
        private TimeSpan _CurrentTimeMessageString { get; set; }


        private void timerMessageStringTick(object sender, EventArgs e)
        {
            _CurrentTimeMessageString = _CurrentTimeMessageString.Add(new TimeSpan(0, 0, 0, -1));
            if (_CurrentTimeMessageString.TotalSeconds <= 1)
            {
                _timerMessageStringEntity.Stop();
                MessageString = null;
            }

            //OnPropertyChanged("CurrentTimeBlock");
        }

        private void NewTimerMessageStringEntity()
        {
            //Создаем таймер отображения монопольных блокировок 
            _timerMessageStringEntity = new DispatcherTimer();
            _timerMessageStringEntity.Tick += new EventHandler(timerMessageStringTick);
            _timerMessageStringEntity.Interval = new TimeSpan(0, 0, 0, 1, 0);
            //*************************************************
        }

        private void StartTimerMessageStringEntity(int spanSecond)
        {
            //Проверяем существование таймера
            if (_timerMessageStringEntity == null)
                Application.Current.Dispatcher.Invoke(NewTimerMessageStringEntity);

            //Запускаем/Сбрасываем таймер отсчета времени жизни блокировки
            _CurrentTimeMessageString = new TimeSpan(0, 0, 0, spanSecond);
            //Включаем таймер просмотра блокировки
            _timerMessageStringEntity.Start();

        }

        #endregion

        #region ErrorTimerString

        private string _ErrorString;

        public string ErrorString
        {
            get => _ErrorString;
            set
            {
                _ErrorString = value;
                if (value != null)
                {
                    StartTimerErrorStringEntity(5);
                }

                OnPropertyChanged("ErrorString");
            }
        }


        private DispatcherTimer _timerErrorStringEntity;
        private TimeSpan _CurrentTimeErrorString { get; set; }


        private void timerErrorStringTick(object sender, EventArgs e)
        {
            _CurrentTimeErrorString = _CurrentTimeErrorString.Add(new TimeSpan(0, 0, 0, -1));
            if (_CurrentTimeErrorString.TotalSeconds <= 1)
            {
                _timerErrorStringEntity.Stop();
                ErrorString = null;

            }
        }

        private void NewTimerErrorStringEntity()
        {
            //Создаем таймер отображения монопольных блокировок 
            _timerErrorStringEntity = new DispatcherTimer();
            _timerErrorStringEntity.Tick += new EventHandler(timerErrorStringTick);
            _timerErrorStringEntity.Interval = new TimeSpan(0, 0, 0, 1, 0);
            //*************************************************
        }

        private void StartTimerErrorStringEntity(int spanSecond)
        {
            //Проверяем существование таймера
            if (_timerErrorStringEntity == null)
                Application.Current.Dispatcher.Invoke(NewTimerErrorStringEntity);

            //Запускаем/Сбрасываем таймер отсчета времени жизни блокировки
            _CurrentTimeErrorString = new TimeSpan(0, 0, 0, spanSecond);
            //Включаем таймер просмотра блокировки
            _timerErrorStringEntity.Start();

        }

        #endregion

        #region CustomSearchPanel

        private string _bufferSearchText;

        public string BufferSearchText
        {
            get
            {
                if (_bufferSearchText is null) _bufferSearchText = string.Empty;

                //проверяем на законченую загрузку грида и если она завершена то присваиваем значения своей строки поиска родной панели
                if (GridCartoonsList.IsLoaded && GridCartoonsList.ChildrenOfType<GridViewSearchPanel>() != null)
                    ((SearchViewModel) GridCartoonsList.ChildrenOfType<GridViewSearchPanel>().FirstOrDefault()
                        ?.DataContext).SearchText = _bufferSearchText;

                return _bufferSearchText;
            }
            set
            {
                _bufferSearchText = value;
                OnPropertyChanged("BufferSearchText");
            }
        }

        #endregion


        #region ClipboardMode

        private bool _clipboardMode;

        /// <summary> Режим работы буффера обмена грида /// </summary>
        public bool ClipboardMode
        {
            get => _clipboardMode;
            set
            {
                _clipboardMode = value;
                OnPropertyChanged("ClipboardMode");
            }
        }

        public ICommand CommandCalendarToToday { get; set; }

        public void OnCalendarToToday(object obj)
        {
            CalendarTo.SelectedDate = DateTime.Today;
            CalendarTo.DisplayDate = DateTime.Today;
            CalendarTo.DisplayMode = DisplayMode.MonthView;
        }

        public ICommand CommandCalendarFromToday { get; set; }

        public void OnCalendarFromToday(object obj)
        {
            CalendarFrom.SelectedDate = DateTime.Today;
            CalendarFrom.DisplayDate = DateTime.Today;
            CalendarFrom.DisplayMode = DisplayMode.MonthView;
        }

        public ICommand CommandSaveOrder { get; set; }
        public Worksheet worksheet = null;
        private Range excelcells;
        public void OnSaveOrder(object obj)
        {
            var df = Directory.GetFiles("Helper\\DATA\\","*UserData.xlsx");
            if (df.Length < 0) return;

            var m_objExcel = new Microsoft.Office.Interop.Excel.Application();
            var m_objBook = m_objExcel.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory+ "Helper\\DATA\\UserData.xlsx", 0, true);
            var firstWorksheet = (Microsoft.Office.Interop.Excel.Worksheet) m_objBook.Worksheets[1];
            var excelRange = firstWorksheet.UsedRange;
            var mass = (object[,]) excelRange.Value[Microsoft.Office.Interop.Excel.XlRangeValueDataType.xlRangeValueDefault];

            var newMass = new object[mass.GetLength(0)+1,mass.GetLength(1)];
            for (var i = 0; i < mass.GetLength(0); i++)
            {
                newMass[i , 0] = i == 0 ? mass[i+1 ,1] : i+1000;
                newMass[i, 1] = mass[i+1 ,2];
                newMass[i, 2] = mass[i+1 ,3];
                newMass[i, 3] = mass[i+1 ,4];
                newMass[i, 4] = mass[i+1 ,5];
                newMass[i, 5] = mass[i+1 ,6];
                newMass[i, 6] = mass[i+1 ,7];
                newMass[i, 7] = mass[i+1 ,8];
                newMass[i, 8] = mass[i+1 ,9];
                newMass[i, 9] = mass[i+1 ,10];
            }
                    
            newMass[newMass.GetLength(0)-1, 0] = newMass.GetLength(0)+1000;
            newMass[newMass.GetLength(0)-1, 1] = SelectedSlots.Index;
            newMass[newMass.GetLength(0)-1, 2] = SelectedCustomTheaterPerformance.Index;
            newMass[newMass.GetLength(0)-1, 3] = NewOrder.Surname;
            newMass[newMass.GetLength(0)-1, 4] = NewOrder.Name;
            newMass[newMass.GetLength(0)-1, 5] = NewOrder.Email;
            newMass[newMass.GetLength(0)-1, 6] = NewOrder.PhoneNumber;
            newMass[newMass.GetLength(0)-1, 7] = NewOrder.CartNumber;
            newMass[newMass.GetLength(0)-1, 8] = NewOrder.Date;
            newMass[newMass.GetLength(0)-1, 9] = NewOrder.CVV;

            var row = newMass.GetUpperBound(0) + 1;
            var column = newMass.GetUpperBound(1) + 1;

            excelcells = firstWorksheet.Range[firstWorksheet.Cells[3, 1], firstWorksheet.Cells[row + 3, newMass.GetUpperBound(1)]];

            if (row > 0 && column > 0)
            {
                excelcells = excelcells.Resize[row, column];
                excelcells.Value = newMass;
            }

            m_objBook.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "Helper\\DATA\\UserData2.xlsx");
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Helper\\DATA\\UserData.xlsx");
            m_objBook.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "Helper\\DATA\\UserData.xlsx");
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Helper\\DATA\\UserData2.xlsx");
            m_objBook.Close();

            ActiveTab = 0;
        }

        public ICommand CommandToggleRightPanel { get; set; }

        private void OnToggleRightPanel(object obj)
        {
            if (obj != null || VisibilityRightPanel == Visibility.Visible)
            {
                int.TryParse(obj?.ToString(), out var index);
                SelectedIndexRightPanel = index;
                WidthRightPanel = 550;
                VisibilityRightPanel = Visibility.Collapsed;
            }
            else
            {
                WidthRightPanel = 40;
                VisibilityRightPanel = Visibility.Visible;
            }
        }

        /// <summary>Изменить режим копирования</summary>
        public ICommand CommandClipboardMode { get; set; }

        private void OnClipboardMode(object obj)
        {
            ClipboardMode = !ClipboardMode;
        }

        /// <summary>Создать спектакль</summary>
        public ICommand CommandCreateNewTheaterPerformance { get; set; }

        private void OnCreateNewTheaterPerformance(object obj)
        {

        }

        /// <summary>Обновить данные</summary>
        public ICommand CommandUpdateData { get; set; }
        private void OnUpdateData(object obj)
        {  
            var m_objExcel = new Microsoft.Office.Interop.Excel.Application();
            Application.Current.Dispatcher.Invoke(()=> { IsBusy = true; });

            try
            {
                Application.Current.Dispatcher.Invoke(() => 
                    { 
                    var df = Directory.GetFiles("Helper\\DATA\\", "*.xlsx");
                    if (df.Length < 0) return;
                    ListTheaterPerformance = new ObservableCollection<TheaterPerformanceVM>();

                    foreach (var t in df)
                    {
                         m_objExcel = new Microsoft.Office.Interop.Excel.Application();
                         var m_objBook = m_objExcel.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory+ t, 0, true);

                        var firstWorksheet = (Microsoft.Office.Interop.Excel.Worksheet) m_objBook.Worksheets[1];
                        var excelRange = firstWorksheet.UsedRange;
                        var mass = (object[,]) excelRange.Value[Microsoft.Office.Interop.Excel.XlRangeValueDataType.xlRangeValueDefault];

                        var fileTitle = "ПорядковйномерНазваниеДатаиВремя";

                        var аа = $"{mass[1, 1]}{mass[1, 2]}{mass[1, 3]}".Replace(" ", "");
                        if ($"{mass[1, 1]}{mass[1, 2]}{mass[1, 3]}".Replace(" ", "") == fileTitle)
                        {
                            for (var i = 2; i <= mass.GetLength(0); ++i)
                            {
                                if (mass[i, 2]is null || mass[i, 2].ToString() == string.Empty)
                                {
                                    m_objBook.Close();
                                    m_objExcel.Quit();
                                    break;
                                }
                                var date = DateTime.Parse(mass[i, 3].ToString(), new CultureInfo("ru-RU"));

                                //Проверяем все ли поля заполнены и имеют необходимый тип
                                if (   mass[i, 1] != null && int.TryParse(mass[i, 1].ToString(), out var index)
                                    && mass[i, 4] != null && int.TryParse(mass[i, 4].ToString(), out var room)
                                    && mass[i, 9] != null && int.TryParse(mass[i, 9].ToString(), out var cost)
                                    && date != DateTime.MinValue
                                )
                                {
                                    ListTheaterPerformance.Add(new TheaterPerformanceVM()
                                    {
                                        Index = index,
                                        Name = mass[i, 2].ToString(),
                                        PlanTime = date,
                                        Room = room,
                                        Description = mass[i, 5].ToString(),
                                        Persons = mass[i, 6].ToString(),
                                        Creators = mass[i, 7].ToString(),
                                        Duration = mass[i, 8].ToString(),
                                        Cost = cost,
                                    });
                                }
                                else
                                {
                                    m_objBook.Close();
                                    m_objExcel.Quit();
                                    ErrorString = "Файл содержит ошибку в строке " + (i + 2);

                                    IsBusy = false;
                                    return;
                                }
                            }
                        }
                   
                        var fileSecondTitle = "ПорядковйномерМестоСпектакль";
                        if ($"{mass[1, 1]}{mass[1, 2]}{mass[1, 3]}".Replace(" ", "") == fileSecondTitle)
                        {
                            for (var i = 2; i <= mass.GetLength(0); ++i)
                            {
                                if (mass[i, 2]is null || mass[i, 2].ToString() == string.Empty)
                                {
                                    m_objBook.Close();
                                    m_objExcel.Quit();
                                    break;
                                }

                                //Проверяем все ли поля заполнены и имеют необходимый тип
                                if (   mass[i, 1] != null && int.TryParse(mass[i, 1].ToString(), out var index)
                                    && mass[i, 2] != null && int.TryParse(mass[i, 2].ToString(), out var slot)
                                )
                                {
                                    if (ListSlots.ContainsKey(slot))
                                        ListSlots[slot].IsFree = false;
                                }
                                else
                                {
                                    m_objBook.Close();
                                    m_objExcel.Quit();
                                    ErrorString = "Файл содержит ошибку в строке " + (i + 2);

                                    IsBusy = false;
                                    return;
                                }
                            }
                        }
                    }

                    //оставляем только те что входят в выбранный промежуток
                    ListCustomTheaterPerformance = new ObservableCollection<TheaterPerformanceVM>(
                        ListTheaterPerformance.Where(t => t.PlanTime > SelectedDateFrom && t.PlanTime < SelectedDateTo));

                },DispatcherPriority.Background);

                Application.Current.Dispatcher.Invoke(()=> { IsBusy = false; });

            }
            catch(Exception exception)
            {
                IsBusy = false;
                m_objExcel.Workbooks.Close();
                m_objExcel.Quit();
            }
        }

        /// <summary> Дабл клик - переход к редактированию поставки с проверкой </summary>
        public ICommand CommandMouseDoubleClickGrid { get; set; }
        public void OnMouseDoubleClickGrid(object obj) 
        {
            if (!(obj is MouseButtonEventArgs e)) return;
            if (!(e.OriginalSource is FrameworkElement originalSender)) return;
            var row = originalSender.ParentOfType<GridViewRow>();

            if (row != null && row.DataContext is TheaterPerformanceVM performance)
            {
                ActiveTab = 1;
            }
        }
        
        public ICommand CommandBackToMain { get; set; }
        public void OnBackToMain(object obj)
        {
            ActiveTab = 0;
        }
        /// <summary>Обработка копирования строки грида</summary>
        public ICommand CommandCopyingCellClipboard { get; set; }

        private void OnCopyingCellClipboard(object obj)
        {
            if (ClipboardMode) return;
            if (obj is GridViewCellClipboardEventArgs e) e.Cancel = true;
        }
        
        public ICommand CommandClearFilterGrid { get; set; }
        private void OnClearFilterGrid(object obj) 
        {
            if (!(obj is RadGridView gv)) return;
            gv.FilterDescriptors.SuspendNotifications();
            gv.SortDescriptors.Clear();
            gv.GroupDescriptors.Clear();
            BufferSearchText = string.Empty;

            foreach (Telerik.Windows.Controls.GridViewColumn column in gv.Columns)
            {
                column.ClearFilters();
            }
            gv.FilterDescriptors.ResumeNotifications();
        }


        /// <summary>Копирование значения текущей ячейуи в буффер обмена</summary>
        public ICommand CommandCopying { get; set; }

        private void OnCopying(object obj)
        {
            
                if (ClipboardMode) return;

                if (obj is GridViewClipboardEventArgs e)
                {
                    e.Cancel = true;

                    if (e.OriginalSource is RadGridView grid)
                    {
                        Clipboard.Clear();
                        Clipboard.SetText(grid.CurrentCell.Value.ToString().Trim());
                    }
                }
        }

        #endregion

        #region ContextMenu

        public ObservableCollection<ModeltemMenu> GetContextMenu(TheaterPerformanceVM sender) {

             var itemContextMenuListOrders = new ObservableCollection<ModeltemMenu>();
             if (sender is null)
             {
                 ErrorString = "Ошибка при выборе строки";
                 return itemContextMenuListOrders;
             }


             //Все запрещаем********************************
            
            var NotEdit = false;

            //**********************************************


            //*********************************************
            return itemContextMenuListOrders;
        }


      #endregion

    }
}

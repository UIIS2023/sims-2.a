using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;

namespace SOSTeam.TravelAgency.WPF.Views.TourGuide.DemoPages
{
    /// <summary>
    /// Interaction logic for AddAppointmentsDemoPage.xaml
    /// </summary>
    public partial class AddAppointmentsDemoPage : Page, INotifyPropertyChanged
    {
        private DispatcherTimer timer;
        private int currentLetterIndex = 0;
        private bool isFilledFirstDate = true; // Flag to track the state of the demo
        private bool isFilledFirstTime = false;
        private bool isAddedStartFirstAppointment = false;
        private bool isFilledSecondDate = false; // Flag to track the state of the demo
        private bool isFilledSecondTime = false;
        private bool isAddedStartSecondAppointment = false;

        private DateTime _start;

        public DateTime Start
        {
            get => _start;
            set
            {
                if (_start != value)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<AppointmentCardViewModel> AppointmentCards { get; set; }

        public AddAppointmentsDemoPage()
        {
            InitializeComponent();
            DataContext = this;
            AppointmentCards = new ObservableCollection<AppointmentCardViewModel>();
            StartDemo();
        }

        private async void StartDemo()
        {
            await Task.Delay(100);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.9); // Set the initial interval to 0.2 seconds
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isFilledFirstDate)
            {
                FillDatePickerLetterByLetter(DatePicker, new DateTime(2024, 5, 25, 0, 0, 0));
            }
            else if (!isFilledFirstTime)
            {
                FillTimePickerLetterByLetter(TimePicker, new DateTime(2024, 5, 25, 18, 30, 0));
            }
            else if (!isAddedStartFirstAppointment)
            {
                SimulateAddingStartCheckpoint();
            }
            else if (!isFilledSecondDate)
            {
                FillDatePickerLetterByLetter(DatePicker, new DateTime(2025, 12, 25, 0, 0, 0));
            }
            else if (!isFilledSecondTime)
            {
                FillTimePickerLetterByLetter(TimePicker, new DateTime(2025, 12, 25, 16, 45, 0));
            }
            else if (!isAddedStartSecondAppointment)
            {
                SimulateAddingStartCheckpoint();
            }

        }

        private async void FillDatePickerLetterByLetter(DatePicker datePicker, DateTime date)
        {
            DatePicker.SelectedDate = date;
            Pause();
        }

        private async void FillTimePickerLetterByLetter(TimePicker timePicker, DateTime time)
        {
            timePicker.Value = time;
            Pause();
            
        }


        private void SimulateAddingStartCheckpoint()
        {
            AddAppointment_OnClick(this, null);
            Pause();
        }

        private async void Pause()
        {
            timer.Stop();
            currentLetterIndex = 0;
            timer.Interval = TimeSpan.FromSeconds(0.9);
            timer.Start();

            if (isFilledFirstDate)
            {
                isFilledFirstDate = false;
            }
            else if (!isFilledFirstTime)
            {
                isFilledFirstTime = true;
            }
            else if (!isAddedStartFirstAppointment)
            {
                isAddedStartFirstAppointment = true;
            }
            else if (!isFilledSecondDate)
            {
                isFilledSecondDate = true;
            }
            else if (!isFilledSecondTime)
            {
                isFilledSecondTime = true;
            }
            else if (!isAddedStartSecondAppointment)
            {
                isAddedStartSecondAppointment = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void AddAppointment_OnClick(object sender, RoutedEventArgs e)
        {
            AppointmentCardViewModel checkpointCard = new AppointmentCardViewModel();
            checkpointCard.Start = Start;

            // Set the initial color
            AddButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(196, 229, 246));

            // Delay for 0.3 seconds
            await Task.Delay(200);

            // Set the final color
            AddButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(211, 211, 211));
            AppointmentCards.Add(checkpointCard);

            // Reset other UI elements
            DatePicker.Text = string.Empty;
            TimePicker.Text = string.Empty;

            await Task.Delay(500);
        }
    }
}

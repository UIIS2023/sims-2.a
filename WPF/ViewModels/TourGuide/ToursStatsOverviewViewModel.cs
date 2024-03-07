using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class ToursStatsOverviewViewModel : ViewModel
    {
        private TourCardViewModel _mostAttendedTourEver;

        public TourCardViewModel MostAttendedTourEver
        {
            get => _mostAttendedTourEver;
            set
            {
                if (_mostAttendedTourEver != value)
                {
                    _mostAttendedTourEver = value;
                    OnPropertyChanged("MostAttendedTourEver");
                }
            }
        }

        private TourCardViewModel _mostAttendedTourOfYear;

        public TourCardViewModel MostAttendedTourOfYear
        {
            get => _mostAttendedTourOfYear;
            set
            {
                if (_mostAttendedTourOfYear != value)
                {
                    _mostAttendedTourOfYear = value;
                    OnPropertyChanged("MostAttendedTourOfYear");
                }
            }
        }

        private string _selectedYear;

        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged("SelectedYear");
                }
            }
        }

        private ObservableCollection<string> _availableYears;

        public ObservableCollection<string> AvailableYears
        {
            get => _availableYears;
            set
            {
                if (_availableYears != value)
                {
                    _availableYears = value;
                    OnPropertyChanged("AvailableYears");
                }
            }

        }

        public User LoggedUser { get; set; }
        public RelayCommand YearSelectionChangedCommand { get; set; }

        private readonly TourCardCreatorViewModel _tourCardCreator;

        private readonly AppointmentService _appointmentService;
        private readonly TourStatsService _tourStatsService;

        public ToursStatsOverviewViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            _tourCardCreator = new TourCardCreatorViewModel();
            var availableYearsCreator = new AvailableYearsCreator();
            _appointmentService = new AppointmentService();
            
            _tourStatsService = new TourStatsService();

            MostAttendedTourEver = new TourCardViewModel();
            MostAttendedTourOfYear = new TourCardViewModel();
            AvailableYears = new ObservableCollection<string>();
            YearSelectionChangedCommand = new RelayCommand(ExecuteYearSelectionChanged, CanExecuteMethod);

            AvailableYears = availableYearsCreator.GetAvailableYears(loggedUser);
            if (AvailableYears.Count > 0)
            {
                SelectedYear = AvailableYears[0];
            }

            FindMostAttendedTourEver();
            FindMostAttendedTourOfYear();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }


        private void FindMostAttendedTourEver()
        {
            Appointment? mostAttendedAppointmentEver = _tourStatsService.FindMostAttendedAppointment(false, string.Empty, LoggedUser);
            MostAttendedTourEver = _tourCardCreator.CreateMostAttendedTour(mostAttendedAppointmentEver);
        }

        private void FindMostAttendedTourOfYear()
        {
            Appointment? mostAttendedAppointmentOfYear = _tourStatsService.FindMostAttendedAppointment(true, SelectedYear, LoggedUser);
            MostAttendedTourOfYear = _tourCardCreator.CreateMostAttendedTour(mostAttendedAppointmentOfYear);
        }

        public void ExecuteYearSelectionChanged(object parameter)
        {
            YearSelectionChanged(parameter, null);
        }

        public void YearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FindMostAttendedTourOfYear();
        }
    }
}

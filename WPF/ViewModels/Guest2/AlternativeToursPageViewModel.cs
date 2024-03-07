using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class AlternativeToursPageViewModel : ViewModel
    {
        private AlternativeToursPage _page;
        private static ToursOverviewWindow _window;
        private Tour _tour;
        private LocationService _locationService;
        private AppointmentService _appointmentService;
        private TourService _tourService;
        private ImageService _imageService;
        public static ObservableCollection<TourViewModel> AlternativeTours { get; set; }
        public User LoggedInUser { get; set; }

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }
        public AlternativeToursPageViewModel(Tour tour, User loggedInUser, ToursOverviewWindow window, AlternativeToursPage page)
        {
            _tour = tour;
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            _tourService = new TourService();
            _imageService = new ImageService();
            AlternativeTours = new ObservableCollection<TourViewModel>();
            BackCommand = new RelayCommand(Execute_BackCommand, CanExecuteMethod);
            FillAlternativeToursList();
            LoggedInUser = loggedInUser;
            _window = window;
            _page = page;
        }
        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
        private void Execute_BackCommand(object obj)
        {
            ToursOverviewWindow window = new ToursOverviewWindow(LoggedInUser);
            window.Show();
            Window.GetWindow(_page).Close();
        }
        private void FindImage(Tour t,string city,string country)
        {
            foreach (Image i in _imageService.GetAll())
            {
                if (i.Type.Equals(ImageType.TOUR) && i.EntityId == t.Id && i.Cover)
                {
                    TourViewModel tourDTO = new TourViewModel(t.Id, t.Name, t.Language, t.Duration, t.MaxNumOfGuests, city, country, LoggedInUser,/* _window,*/ i.Path);
                    AlternativeTours.Add(tourDTO);
                    break;
                }
            }
        }

        private void FillAlternativeToursList()
        {
            List<Tour> tours = GetSameLocatedTours();

            string city = _tourService.GetTourCity(_tour);
            string country = _tourService.GetTourCountry(_tour);

            foreach (Tour tour in tours)
            {
                foreach (Appointment appointment in _appointmentService.GetAll())
                {
                    if (appointment.TourId == tour.Id && appointment.Occupancy != tour.MaxNumOfGuests)
                    {
                        FindImage(tour,city,country);
                    }
                }
            }
        }

        private List<Tour> GetSameLocatedTours()
        {
            List<Tour> tours = new List<Tour>();
            tours = _tourService.GetSameLocatedTours(_tour.LocationId);
            tours.Remove(_tour);
            return tours;
        }
    }
}

using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Image = SOSTeam.TravelAgency.Domain.Models.Image;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SOSTeam.TravelAgency.Commands;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class MyReservationsPageViewModel : ViewModel
    {
        public static User LoggedInUser { get; set; }
        public static ObservableCollection<MyReservationViewModel> Reservations { get; set; }

        private TourService _tourService;
        private LocationService _locationService;
        private GuestAttendanceService _guestAttendanceService;
        private ReservationService _reservationService;
        private AppointmentService _appointmentService;
        private ImageService _imageService;

        private RelayCommand _showReportCommand;
        public RelayCommand ShowReportCommand
        {
            get { return _showReportCommand; }
            set
            {
                _showReportCommand = value;
            }
        }
        public MyReservationsPageViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            ShowReportCommand = new RelayCommand(Execute_ShowCommand, CanExecuteMethod);
            InitializeServices();
            Reservations = new ObservableCollection<MyReservationViewModel>();
            FillReservationsList();
        }

        private void Execute_ShowCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void FillReservationsList()
        {
            foreach(var reservation in _reservationService.GetAll())
            {
                if(reservation.UserId == LoggedInUser.Id)
                {
                    Appointment reservedTourAppointment = _appointmentService.GetById(reservation.AppointmentId);
                    Tour reservedTour = _tourService.GetById(reservedTourAppointment.TourId);
                    Location reservedTourLocation = _locationService.GetById(reservedTour.LocationId);
                    Image reservedTourImage = GetReservedTourImage(reservedTour);
                    MyReservationViewModel reservationView = new MyReservationViewModel(reservation.Id,reservedTour.Name, _locationService.GetFullName(reservedTourLocation), reservedTour.Language, reservedTourAppointment.Start, reservation.TouristNum,reservedTourImage.Path);
                    Reservations.Add(reservationView);
                }
            }
        }

        private Image GetReservedTourImage(Tour reservedTour)
        {
            foreach (Image image in _imageService.GetAll())
            {
                if (image.EntityId == reservedTour.Id && image.Type.Equals(ImageType.TOUR) && image.Cover)
                {
                    return image;
                }
            }
            return null;
        }


        private void InitializeServices()
        {
            _tourService = new TourService();
            _locationService = new LocationService();
            _guestAttendanceService = new GuestAttendanceService();
            _reservationService = new ReservationService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}

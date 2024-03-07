using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;


namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class HomePageViewModel : ViewModel
    {
        #region Services

        private readonly AppointmentService _appointmentService;
        private readonly ReservationService _reservationService;
        private readonly VoucherService _voucherService;

        #endregion

        #region Commands

        public RelayCommand CancelTourCommand { get; set; }
        public RelayCommand ShowTodayToursCommand { get; set; }

        #endregion


        private ObservableCollection<TourCardViewModel> _tourCards;
        public ObservableCollection<TourCardViewModel> TourCards
        {
            get => _tourCards;
            set
            {
                if (_tourCards != value)
                {
                    _tourCards = value;
                    OnPropertyChanged("TourCards");
                }
            }
        }

        public User LoggedUser { get; set; }
        private readonly DispatcherTimer _timer;

        public HomePageViewModel(User loggedUser, DispatcherTimer timer)
        {
            LoggedUser = loggedUser;
            _timer = timer;
            _appointmentService = new AppointmentService();
            _reservationService = new ReservationService();
            _voucherService = new VoucherService();

            _appointmentService.SetExpiredAppointments(loggedUser.Id);

            var tourCardCreator = new TourCardCreatorViewModel();
            _tourCards = tourCardCreator.CreateCards(loggedUser, CreationType.ALL);

            _timer.Tick += UpdateTourCards;

            CancelTourCommand = new RelayCommand(CancelTourClick, CanExecuteMethod);
            ShowTodayToursCommand = new RelayCommand(TodayToursClick, CanExecuteMethod);
        }

        private void UpdateTourCards(object sender, EventArgs e)
        {
            foreach (var tourCard in TourCards)
            {
                var appointment = _appointmentService.GetById(tourCard.AppointmentId);
                tourCard.SetAppointmentStatusAndBackground(appointment);
            }
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void CancelTourClick(object sender)
        {
            var message = "Are you sure you want to cancel the tour?\nIf you cancel the tour," +
                          " all users with a reservation for this tour will receive a voucher.";
            var result = App.TourGuideNavigationService.GetMessageBoxResult(message);

            var selectedAppointment = sender as TourCardViewModel;

            if (result == true)
            {
                _appointmentService.Delete(selectedAppointment.AppointmentId);
                TourCards.Remove(selectedAppointment);
                _voucherService.GiveVouchers(_reservationService.GetAllByAppointmentId(selectedAppointment.AppointmentId));
            }
        }

        private void TodayToursClick(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("TodayTours", LoggedUser);
        }

    }
}

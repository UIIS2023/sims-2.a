using SOSTeam.TravelAgency.Application.Services;
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
    public class FinishedTourNotificationPageViewModel : ViewModel
    {
        public static User LoggedInUser { get; set; }
        private FinishedToursNotificationPage _page;

        private readonly ReservationService _reservationService;
        private readonly AppointmentService _appointmentSevice;
        private readonly TourService _tourService;

        public static ObservableCollection<FinishedTourViewModel> FinishedTours { get; set; }

        public FinishedTourNotificationPageViewModel(FinishedToursNotificationPage page, User loggedInUser)
        {
            _reservationService = new ReservationService();
            _appointmentSevice= new AppointmentService();
            _tourService = new TourService();
            _page = page;
            LoggedInUser = loggedInUser;
            FinishedTours = new ObservableCollection<FinishedTourViewModel>();
            FillFinishedToursList();
        }

        private void FillFinishedToursList()
        {
            foreach (Reservation reservation in _reservationService.GetAll())
            {
                if (reservation.Presence && _appointmentSevice.GetById(reservation.AppointmentId).Finished && reservation.Reviewed == false)
                {
                    FinishedTours.Add(new FinishedTourViewModel(reservation.Id, reservation.AppointmentId, LoggedInUser, _tourService.GetTourName(_appointmentSevice.GetById(reservation.AppointmentId).TourId)));
                }
            }
        }
    }
}

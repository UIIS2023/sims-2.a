using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class NewToursNotificationPageViewModel
    {
        public static User LoggedInUser { get; set; }
        public static ObservableCollection<NewToursViewModel> NewTours { get; set; }

        private readonly TourService _tourService;
        private readonly TourRequestService _tourRequestService;
        private readonly NewTourNotificationService _newTourNotificationService;
        private readonly AppointmentService _appointmentService;
        

        public NewToursNotificationPageViewModel(User loggedInUser)
        {
            _appointmentService = new AppointmentService();
            _newTourNotificationService = new NewTourNotificationService();
            _tourService = new TourService();
            _tourRequestService= new TourRequestService();
            LoggedInUser= loggedInUser;
            NewTours = new ObservableCollection<NewToursViewModel>();
            FillNewToursList();
        }

        private void FillNewToursList()
        {
            List<Tour> potentialToursForShowing = GetPotentialTours();
            List<TourRequest> invalidRequests = _tourRequestService.GetAllInvalid();

            foreach(var request in invalidRequests)
            {
                if(request.UserId == LoggedInUser.Id)
                {
                    AddToNewToursList(request,potentialToursForShowing);
                }
            }

            NewTours = new ObservableCollection<NewToursViewModel>(NewTours.DistinctBy(v => v.TourId));
        }

        private void AddToNewToursList(TourRequest request,List<Tour> potentialToursForShowing)
        {
            foreach(var tour in potentialToursForShowing)
            {
                if((request.City.Equals(_tourService.GetTourCity(tour)) && request.Country.Equals(_tourService.GetTourCountry(tour))) || request.Language.Equals(tour.Language))
                {
                    NewTours.Add(new NewToursViewModel(LoggedInUser, tour.Id, tour.Name));
                }
            }
        }

        private List<Tour> GetPotentialTours()
        {
            List<Tour> potentialToursForShowing = new List<Tour>();
            List<Appointment> notificationAppointments = _appointmentService.GetNotificationAppointments(LoggedInUser);

            foreach(var appointments in notificationAppointments)
            {
                potentialToursForShowing.Add(_tourService.GetById(appointments.TourId));
            }

            return potentialToursForShowing;
        }

    }
}

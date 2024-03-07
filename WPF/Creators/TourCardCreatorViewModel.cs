using System.Collections.Generic;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System.Collections.ObjectModel;


namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public enum CreationType { ALL = 0, TODAY = 1, FINISHED = 2}
    public class TourCardCreatorViewModel
    {
        #region Services

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly AppointmentService _appointmentService;
        private readonly ImageService _imageService;

        #endregion

        public TourCardCreatorViewModel()
        {
            _tourService = new TourService();
            _locationService = new LocationService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
        }

        public ObservableCollection<TourCardViewModel> CreateCards(User loggedUser, CreationType type)
        {
            var appointments = GetAppointmentsByUsageType(loggedUser, type);

            var tourCards = new ObservableCollection<TourCardViewModel>();
            foreach (var appointment in appointments)
            {
                foreach (var tour in _tourService.GetAll())
                {
                    if (appointment.TourId == tour.Id)
                    {
                        var tourCard = new TourCardViewModel();

                        SetTourAndAppointment(tourCard, appointment, tour);

                        SetLocation(tour, tourCard);

                        SetCoverImage(tour, tourCard);

                        SetCanStartAppointment(tourCard, appointment, loggedUser);

                        tourCard.SetCanCancel(appointment);

                        tourCards.Add(tourCard);
                    }
                }
            }
            return tourCards;
        }

        public ObservableCollection<TourCardViewModel> CreateCardsPerYear(User loggedUser, string selectedYear)
        {
            var tourCards = new ObservableCollection<TourCardViewModel>();

            foreach (var appointment in _appointmentService.GetAllFinishedByUserId(loggedUser.Id))
            {
                foreach (var tour in _tourService.GetAll())
                {
                    if (appointment.TourId == tour.Id && appointment.Start.Year.ToString() == selectedYear)
                    {
                        var tourCard = new TourCardViewModel();
                        SetTourAndAppointment(tourCard, appointment, tour);
                        SetLocation(tour, tourCard);

                        tourCards.Add(tourCard);
                    }
                }
            }

            return tourCards;
        }

        private List<Appointment> GetAppointmentsByUsageType(User loggedUser, CreationType type)
        {
            List<Appointment> appointments;
            switch (type)
            {
                case CreationType.ALL:
                    appointments = _appointmentService.GetAllByUserId(loggedUser.Id);
                    break;
                case CreationType.TODAY:
                    appointments = _appointmentService.GetTodayAppointmentsByUserId(loggedUser.Id);
                    break;
                default:
                    appointments = _appointmentService.GetAllFinishedByUserId(loggedUser.Id);
                    break;
            }

            return appointments;
        }

        private void SetCoverImage(Tour tour, TourCardViewModel tourCard)
        {
            var image = _imageService.GetTourCover(tour.Id);
            tourCard.SetCoverImage(image);
        }

        private void SetLocation(Tour tour, TourCardViewModel tourCard)
        {
            var location = _locationService.GetById(tour.LocationId);
            tourCard.SetLocation(location);
        }

        private void SetTourAndAppointment(TourCardViewModel tourCard, Appointment appointment, Tour tour)
        {
            tourCard.AppointmentId = appointment.Id;
            tourCard.Start = appointment.Start;
            tourCard.TourId = tour.Id;
            tourCard.Name = tour.Name;
            tourCard.SetAppointmentStatusAndBackground(appointment);
        }

        private void SetCanStartAppointment(TourCardViewModel tourCard, Appointment appointment, User loggedUser)
        {
            var activeAppointment = _appointmentService.GetActiveByUserId(loggedUser.Id);
            tourCard.SetCanStart(appointment, activeAppointment);
        }

        public TourCardViewModel? CreateMostAttendedTour(Appointment? mostAttendedAppointment)
        {
            if (mostAttendedAppointment == null)
            {
                var notExistsCard = new TourCardViewModel
                {
                    Name = "Not exists!",
                    CoverImagePath = "/Resources/Images/Tours/unknown_cover.jpg"
                };
                return notExistsCard;
            }
            var tour = _tourService.GetById(mostAttendedAppointment.TourId);
            var location = _locationService.GetById(tour.LocationId);
            var tourCard = new TourCardViewModel
            {
                TourId = tour.Id,
                AppointmentId = mostAttendedAppointment.Id,
                LocationId = tour.LocationId,
                Start = mostAttendedAppointment.Start,
                Name = tour.Name,
            };
            tourCard.SetAppointmentStatusAndBackground(mostAttendedAppointment);
            tourCard.SetLocation(location);
            tourCard.SetCoverImage(_imageService.GetTourCover(tourCard.TourId));

            return tourCard;
        }

    }
}

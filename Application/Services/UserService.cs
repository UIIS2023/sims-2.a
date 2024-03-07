using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository = Injector.CreateInstance<IUserRepository>();

        public UserService() { }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public void Save(User user)
        {
            _userRepository.Save(user);
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }

        public User GetByUsername(string username)
        {
           return _userRepository.GetByUsername(username);
        }

        public List<User> GetAllSignificantUsers(Location forumLocation)
        {
            List<User> returnList = new List<User>();
            foreach (User user in FindAllSignificantOwners(forumLocation))
            {
                returnList.Add(user);
            }
            foreach (User user in FindAllSignificantGuests(forumLocation))
            {
                returnList.Add(user);
            }

            return returnList;
        }

        private List<User> FindAllSignificantGuests(Location forumLocation)
        {
            List<User> returnList = new List<User>();
            foreach(User user in AnalyzeAccommodationReservations(forumLocation))
            {
                returnList.Add(user);
            }
            foreach (User user in AnalyzeTourReservations(forumLocation))
            {
                returnList.Add(user);
            }
            
            return returnList;
        }

        private List<User> AnalyzeAccommodationReservations(Location forumLocation)
        {
            List<User> significantUsers = new List<User>();
            AccommodationReservationService reservationService = new AccommodationReservationService();
            AccommodationService accommodationService = new AccommodationService();
            UserService userService = new UserService();
            foreach (var reservation in reservationService.GetAll())
            {
                Accommodation accommodation = accommodationService.GetById(reservation.AccommodationId);
                if (accommodation.LocationId == forumLocation.Id)
                {
                    User user = userService.GetById(reservation.UserId);
                    significantUsers.Add(user);
                }
            }

            return significantUsers;
        }

        private List<User> AnalyzeTourReservations(Location forumLocation)
        {
            List<User> significantUsers = new List<User>();
            TourService tourService = new TourService();
            AppointmentService appointmentService = new AppointmentService();
            ReservationService reservationService = new ReservationService();
            UserService userService = new UserService();
            foreach (var reservation in reservationService.GetAll())
            {
                Appointment appointment = appointmentService.GetById(reservation.AppointmentId);
                Tour tour = tourService.GetById(appointment.TourId);
                if (tour.LocationId == forumLocation.Id && reservation.Presence)
                {
                    User user = userService.GetById(reservation.UserId);
                    significantUsers.Add(user);
                }
            }

            return significantUsers;
        }

        private List<User> FindAllSignificantOwners(Location forumLocation)
        {
            List<User> significantUsers = new List<User>();
            AccommodationService accommodationService = new AccommodationService();
            UserService userService = new UserService();
            foreach (var accommodation in accommodationService.GetAll())
            {
                if (accommodation.LocationId == forumLocation.Id)
                {
                    User owner = userService.GetById(accommodation.OwnerId);
                    significantUsers.Add(owner);
                }
            }

            return significantUsers;
        }
    }
}

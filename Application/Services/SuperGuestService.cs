using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class SuperGuestService
    {
        private readonly ISuperGuestRepository superGuestRepository = Injector.CreateInstance<ISuperGuestRepository>();

        public SuperGuestService() { }

        public void Delete(int id)
        {
            superGuestRepository.Delete(id);
        }

        public List<SuperGuest> GetAll()
        {
            return superGuestRepository.GetAll();
        }

        public SuperGuest GetById(int id)
        {
            return superGuestRepository.GetById(id);
        }

        public void Save(SuperGuest superGuest)
        {
            superGuestRepository.Save(superGuest);
        }

        public void Update(SuperGuest superGuest)
        {
            superGuestRepository.Update(superGuest);
        }

        private void ClearSuperGuestCSV()
        {
            List<SuperGuest> _superGuests = superGuestRepository.GetAll();
            if (_superGuests.Count > 0)
            {
                foreach (var guest in _superGuests)
                {
                    superGuestRepository.Delete(guest.Id);
                }
            }
        }

        private int CalculateUnusedBonusPoints(User user, int points)
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            foreach (var reservation in reservationService.GetAll())
            {
                if (reservation.UserId == user.Id && reservation.FirstDay.Year == DateTime.Today.Year && points > 0)
                {
                    points--;
                }
            }
            return points;
        }

        private int InitializeBonusPoints(int resNumber)
        {
            int points = 0;
            if (resNumber >= 10) points = 5;
            return points;
        }

        private bool IntializeSuperStatus(int resNumber)
        {
            bool isSuper = false;
            if (resNumber >= 10) isSuper = true;
            return isSuper;
        }

        public void CreateSuperGuestAccounts()
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            UserService userService = new UserService();
            ClearSuperGuestCSV();
            List<User> _users = userService.GetAll();

            foreach (User user in _users)
            {
                if (user.Role == Roles.GOST1)
                {
                    int resNumber = reservationService.FindLastYearReservationsNumber(user);
                    int points = InitializeBonusPoints(resNumber);
                    bool isSuper = IntializeSuperStatus(resNumber);
                    points = CalculateUnusedBonusPoints(user, points);
                    SuperGuest superGuest = new SuperGuest(user.Id, user.Username, points, resNumber, isSuper);
                    Save(superGuest);
                }
            }
        }
    }
}

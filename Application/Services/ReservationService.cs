using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest2;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository = Injector.CreateInstance<IReservationRepository>();
        private readonly IAppointmentRepository _appointmentRepository = Injector.CreateInstance<IAppointmentRepository>();
        public ReservationService() { }

        public void Delete(int id)
        {
            _reservationRepository.Delete(id);
        }

        public List<Reservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }

        public Reservation GetById(int id)
        {
            return _reservationRepository.GetById(id);
        }

        public void Update(Reservation reservation)
        {
            _reservationRepository.Update(reservation);
        }

        public void Save(Reservation reservation)
        {
            _reservationRepository.Save(reservation);
        }

        public void SetPresence(int id)
        {
            _reservationRepository.SetPresence(id);
        }

        public void Reviewed(int id)
        {
            _reservationRepository.Reviewed(id);
        }

        public List<Reservation> GetAllByAppointmentId(int id)
        {
            return _reservationRepository.GetAllByAppointmentId(id);
        }

        public Reservation CreateReservation(AppoitmentOverviewViewModel selected, User loggedInUser, int touristNum, float averageAge, int voucherId = -1)
        {
            Reservation newReservation;
           foreach (Appointment a in _appointmentRepository.GetAll())
           {
               if (selected.TourId == a.TourId && selected.StartDateTime == a.Start)
               {
                   a.Occupancy += touristNum;
                  // selected.Ocupancy += touristNum;
                   _appointmentRepository.Update(a);
                   newReservation = new Reservation(touristNum,averageAge, loggedInUser.Id, a.Id,voucherId);
                   _reservationRepository.Save(newReservation);
                   return newReservation;
                }
           }
            return null;
        }

        public Reservation FindReservationWhereUserIsPresent(User loggedInUser)
        {
            foreach (Reservation reservation in _reservationRepository.GetAll())
            {
                if (reservation.UserId == loggedInUser.Id && reservation.Presence)
                {
                    return reservation;
                }
            }
            return null;
        }

        public List<Reservation> GetFulfilledReservations(User loggedInUser)
        {
            List<Reservation> reservations = new List<Reservation>();
            foreach(var reservation in _reservationRepository.GetAll())
            {
                if(reservation.UserId == loggedInUser.Id && reservation.Presence)
                {
                    reservations.Add(reservation);
                }
            }
            return reservations;
        }
    }
}

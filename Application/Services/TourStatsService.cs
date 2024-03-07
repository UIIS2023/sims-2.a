using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class TourStatsService
    {
        private readonly AppointmentService _appointmentService;
        private readonly ReservationService _reservationService;

        public TourStatsService()
        {
            _appointmentService = new AppointmentService();
            _reservationService = new ReservationService();
        }

        public Appointment? FindMostAttendedAppointment(bool isPerYear, string year, User loggedUser)
        {
            int maxOfPresenceGuests = 0;
            Appointment? mostAttendedAppointment = null;
            var finishedAppointments = _appointmentService.GetAllFinishedByUserId(loggedUser.Id);

            foreach (var appointment in finishedAppointments)
            {
                if (!isPerYear || appointment.Start.Year.ToString() == year)
                {
                    int currentPresenceGuests = 0;

                    var reservations = _reservationService.GetAllByAppointmentId(appointment.Id);

                    foreach (var reservation in reservations)
                    {
                        if (reservation.Presence)
                        {
                            currentPresenceGuests += reservation.TouristNum;
                        }
                    }

                    if (currentPresenceGuests > maxOfPresenceGuests)
                    {
                        maxOfPresenceGuests = currentPresenceGuests;
                        mostAttendedAppointment = appointment;
                    }
                }
            }

            return mostAttendedAppointment;
        }

        public (int, int, int) CountNumOfGuestsByAgeGroup(int appointmentId)
        {
            var numOfGuestsByAgeGroup = (0, 0, 0);
            foreach (var reservation in _reservationService.GetAllByAppointmentId(appointmentId))
            {
                if (reservation.Presence)
                {
                    if (reservation.AverageAge <= 18)
                    {
                        numOfGuestsByAgeGroup.Item1 += reservation.TouristNum;
                    }
                    else if (reservation.AverageAge <= 50)
                    {
                        numOfGuestsByAgeGroup.Item2 += reservation.TouristNum;
                    }
                    else
                    {
                        numOfGuestsByAgeGroup.Item3 += reservation.TouristNum;
                    }
                }
            }

            return numOfGuestsByAgeGroup;
        }

        public (double, double) GetPercentsOfGuestAttendancesVoucher(int appointmentId)
        {
            float withVoucher = 0;
            float withoutVoucher = 0;

            foreach (var reservation in _reservationService.GetAllByAppointmentId(appointmentId))
            {
                if (reservation.Presence)
                {
                    if (reservation.VoucherId != -1)
                    {
                        withVoucher += reservation.TouristNum;
                    }
                    else
                    {
                        withoutVoucher += reservation.TouristNum;
                    }
                    
                }
            }
            return (withVoucher, withoutVoucher);
        }

    }
}

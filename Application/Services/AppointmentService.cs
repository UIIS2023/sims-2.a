using System;
using System.Collections.Generic;
using System.Linq;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly TourService _tourService;
        private readonly NewTourNotificationService _newTourNotificationService;
        public AppointmentService()
        {
            _appointmentRepository = Injector.CreateInstance<IAppointmentRepository>();
            _tourService = new TourService();
            _newTourNotificationService = new NewTourNotificationService();
        }

        public void Delete(int id)
        {
            _appointmentRepository.Delete(id);
        }

        public List<Appointment> GetAll()
        {
            return _appointmentRepository.GetAll();
        }

        public List<Appointment> GetAllByUserId(int userId)
        {
            return _appointmentRepository.GetAllByUserId(userId);
        }

        public Appointment? GetById(int id)
        {
            return _appointmentRepository.GetById(id);
        }

        public void Save(Appointment appointment)
        {
            _appointmentRepository.Save(appointment);
        }

        public void SaveAll(List<Appointment> appointments)
        {
            _appointmentRepository.SaveAll(appointments);
        }

        public void Update(Appointment appointment)
        {
            _appointmentRepository.Update(appointment);
        }
        public List<Appointment> GetTodayAppointmentsByUserId(int id)
        {
            return _appointmentRepository.GetAllByUserId(id)
                .FindAll(a => a.Start.Date == DateTime.Today);
        }

        public Appointment? GetActiveByUserId(int id)
        {
            return _appointmentRepository.GetAllByUserId(id).Find(a => a.IsActive());
        }

        public List<Appointment> GetAllFinishedByUserId(int id)
        {
            return _appointmentRepository.GetAllByUserId(id).FindAll(a => a.IsFinished());
        }

        public void StartAppointment(int id)
        {
            var appointment = _appointmentRepository.GetById(id);
            if (appointment == null) { return; }
            appointment.Started = true;
            _appointmentRepository.Update(appointment);
        }

        public void FinishAppointment(int id)
        {
            var appointment = _appointmentRepository.GetById(id);
            if(appointment == null) { return; }
            appointment.Finished = true;
            _appointmentRepository.Update(appointment);
        }

        public List<Appointment> GetScheduledAppointments(DateTime? minDate, DateTime? maxDate, int userId)
        {
            var notStartedAppointments = _appointmentRepository
                .GetAllByUserId(userId)
                .FindAll(a => !a.Started && !a.Finished);

            var sortedScheduledAppointments = notStartedAppointments
                .Where(a => a.Start >= minDate && a.Start <= maxDate)
                .OrderBy(a => a.Start)
                .ToList();

            return sortedScheduledAppointments;
        }

        public void SetExpiredAppointments(int userId)
        {
            foreach (var appointment in GetAllByUserId(userId))
            {
                var tour = _tourService.GetById(appointment.TourId);
                if(tour == null) { return; }

                if (appointment.IsExpired(tour.Duration))
                {
                    FinishAppointment(appointment.Id);
                }
            }
        }

        public bool CheckAvailableAppointments(Tour tour)
        {
            foreach(Appointment appointment in _appointmentRepository.GetAll()) 
            {
                if(tour.Id == appointment.TourId)
                {
                    if(appointment.Occupancy < tour.MaxNumOfGuests)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Appointment> GetNotificationAppointments(User loggedInUser)
        {
            List<Appointment> notificationAppointments = new List<Appointment>();
            foreach (var newTourNotification in _newTourNotificationService.GetAllByGuestId(loggedInUser.Id))
            {
                notificationAppointments.Add(_appointmentRepository.GetById(newTourNotification.AppointmentId));
            }
            return notificationAppointments;
        }

        public int GetTourGuideId(int tourId)
        {
            foreach(var appointment in _appointmentRepository.GetAll())
            {
                if(appointment.TourId == tourId)
                {
                    return appointment.UserId;
                }
            }
            return -1;
        }
    }
}

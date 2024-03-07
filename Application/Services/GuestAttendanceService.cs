using System.Collections.Generic;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class GuestAttendanceService
    {
        private readonly IGuestAttendanceRepository _guestAttendanceRepository;

        public GuestAttendanceService()
        {
            _guestAttendanceRepository = Injector.CreateInstance<IGuestAttendanceRepository>();
        }

        public void Delete(int id)
        {
            _guestAttendanceRepository.Delete(id);
        }

        public List<GuestAttendance> GetAll()
        {
            return _guestAttendanceRepository.GetAll();
        }

        public GuestAttendance? GetById(int id)
        {
            return  _guestAttendanceRepository.GetById(id);
        }

        public List<GuestAttendance> GetByUserId(int id)
        {
            return _guestAttendanceRepository.GetByUserId(id);
        }
        public List<GuestAttendance> GetAllByActivityId(int id)
        {
            return _guestAttendanceRepository.GetAllByActivityId(id);
        }

        public void Save(GuestAttendance guestAttendance)
        {
            _guestAttendanceRepository.Save(guestAttendance);
        }

        public void SaveAll(List<GuestAttendance> guestAttendances)
        {
            _guestAttendanceRepository.SaveAll(guestAttendances);
        }

        public void Update(GuestAttendance guestAttendance)
        {
            _guestAttendanceRepository.Update(guestAttendance);
        }

        public void CreateAttendanceQueries(List<Reservation> reservations, CheckpointActivity activeCheckpoint, string checkpointName)
        {
            var guestAttendances = new List<GuestAttendance>();
            foreach (var reservation in reservations)
            {
                var guestAttendance = new GuestAttendance
                {
                    UserId = reservation.UserId,
                    ReservationId = reservation.Id,
                    CheckpointActivityId = activeCheckpoint.Id,
                    Presence = GuestPresence.UNKNOWN,
                    Message = "Da li ste bili prisutni na čekpointu: " + checkpointName + "?"
                };
                guestAttendances.Add(guestAttendance);
            }

            if (guestAttendances.Count > 0)
            {
                SaveAll(guestAttendances);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private const string FilePath = "../../../Resources/Data/appointments.csv";

        private readonly Serializer<Appointment> _serializer;

        private List<Appointment> _appointments;

        public AppointmentRepository()
        {
            _serializer = new Serializer<Appointment>();
            _appointments = _serializer.FromCSV(FilePath);
        }

        public void Delete(int id)
        {
            _appointments = _serializer.FromCSV(FilePath);
            Appointment founded = _appointments.Find(d => d.Id == id) ?? throw new ArgumentException();
            _appointments.Remove(founded);
            _serializer.ToCSV(FilePath, _appointments);
        }

        public List<Appointment> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<Appointment> GetAllByUserId(int id)
        {
            _appointments = _serializer.FromCSV(FilePath);
            return _appointments.FindAll(a => a.UserId == id);
        }

        public Appointment? GetById(int id)
        {
            _appointments = _serializer.FromCSV(FilePath);
            return _appointments.Find(a => a.Id == id);
        }

        public int NextId()
        {
            _appointments = _serializer.FromCSV(FilePath);
            if (_appointments.Count < 1)
            {
                return 1;
            }
            return _appointments.Max(d => d.Id) + 1;
        }

        public void Save(Appointment appointment)
        {
            appointment.Id = NextId();
            _appointments = _serializer.FromCSV(FilePath);
            _appointments.Add(appointment);
            _serializer.ToCSV(FilePath, _appointments);
        }

        public void SaveAll(List<Appointment> appointments)
        {
            foreach (var appointment in appointments)
            {
                Save(appointment);
            }
        }

        public void Update(Appointment appointment)
        {
            _appointments = _serializer.FromCSV(FilePath);
            Appointment current = _appointments.Find(d => d.Id == appointment.Id) ?? throw new ArgumentException();
            int index = _appointments.IndexOf(current);
            _appointments.Remove(current);
            _appointments.Insert(index, appointment);
            _serializer.ToCSV(FilePath, _appointments);
        }

        public void UpdateAll(List<Appointment> appointments)
        {
            foreach (var appointment in appointments)
            {
                Update(appointment);
            }
        }
    }
}

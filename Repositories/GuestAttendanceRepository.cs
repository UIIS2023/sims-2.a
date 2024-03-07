using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class GuestAttendanceRepository : IGuestAttendanceRepository
    {
        private const string FilePath = "../../../Resources/Data/guestsAttendances.csv";

        private readonly Serializer<GuestAttendance> _serializer;

        private List<GuestAttendance> _guestsAttendances;

        public GuestAttendanceRepository()
        {
            _serializer = new Serializer<GuestAttendance>();
            _guestsAttendances = _serializer.FromCSV(FilePath);
        }

        public void Delete(int id)
        {
            _guestsAttendances = _serializer.FromCSV(FilePath);
            GuestAttendance founded = _guestsAttendances.Find(c => c.Id == id) ?? throw new ArgumentException();
            _guestsAttendances.Remove(founded);
            _serializer.ToCSV(FilePath, _guestsAttendances);
        }

        public List<GuestAttendance> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<GuestAttendance> GetAllByActivityId(int id)
        {
            _guestsAttendances = _serializer.FromCSV(FilePath);
            return _guestsAttendances.FindAll(a => a.CheckpointActivityId == id);
        }

        public GuestAttendance? GetById(int id)
        {
            _guestsAttendances = _serializer.FromCSV(FilePath);
            return _guestsAttendances.Find(a => a.Id == id);
        }

        public List<GuestAttendance> GetByUserId(int id)
        {
            _guestsAttendances = _serializer.FromCSV(FilePath);
            return _guestsAttendances.FindAll(a => a.UserId == id) ?? throw new ArgumentException();
        }

        public int NextId()
        {
            _guestsAttendances = _serializer.FromCSV(FilePath);
            if (_guestsAttendances.Count < 1)
            {
                return 1;
            }
            return _guestsAttendances.Max(d => d.Id) + 1;
        }

        public void Save(GuestAttendance attendance)
        {
            attendance.Id = NextId();
            _guestsAttendances = _serializer.FromCSV(FilePath);
            _guestsAttendances.Add(attendance);
            _serializer.ToCSV(FilePath, _guestsAttendances);
        }

        public void SaveAll(List<GuestAttendance> attendances)
        {
            foreach (GuestAttendance attendance in attendances)
            {
                Save(attendance);
            }
        }

        public void Update(GuestAttendance attendance)
        {
            _guestsAttendances = _serializer.FromCSV(FilePath);
            GuestAttendance current = _guestsAttendances.Find(a => a.Id == attendance.Id) ?? throw new ArgumentException();
            int index = _guestsAttendances.IndexOf(current);
            _guestsAttendances.Remove(current);
            _guestsAttendances.Insert(index, attendance);
            _serializer.ToCSV(FilePath, _guestsAttendances);
        }
    }
}

using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Repositories
{
    public class GuestAccommodationMarkRepository : IGuestAccommodationMarkRepository
    {
        private const string FilePath = "../../../Resources/Data/guestAccommodationMarks.csv";
        private readonly Serializer<GuestAccommodationMark> _serializer;
        private List<GuestAccommodationMark> _requests;

        public GuestAccommodationMarkRepository()
        {
            _serializer = new Serializer<GuestAccommodationMark>();
            _requests = new List<GuestAccommodationMark>();
        }

        public void Update(GuestAccommodationMark guestAccommodationMark)
        {
            _requests = _serializer.FromCSV(FilePath);
            GuestAccommodationMark current = _requests.Find(d => d.Id == guestAccommodationMark.Id) ?? throw new ArgumentException();
            int index = _requests.IndexOf(current);
            _requests.Remove(current);
            _requests.Insert(index, guestAccommodationMark);
            _serializer.ToCSV(FilePath, _requests);
        }

        public GuestAccommodationMark GetById(int id)
        {
            _requests = _serializer.FromCSV(FilePath);
            return _requests.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public List<GuestAccommodationMark> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(GuestAccommodationMark guestAccommodationMark)
        {
            guestAccommodationMark.Id = NextId();
            _requests = _serializer.FromCSV(FilePath);
            _requests.Add(guestAccommodationMark);
            _serializer.ToCSV(FilePath, _requests);
        }

        public void Delete(int id)
        {
            _requests = _serializer.FromCSV(FilePath);
            GuestAccommodationMark found = _requests.Find(t => t.Id == id) ?? throw new ArgumentException();
            _requests.Remove(found);
            _serializer.ToCSV(FilePath, _requests);
        }

        public int NextId()
        {
            _requests = _serializer.FromCSV(FilePath);
            if (_requests.Count < 1)
            {
                return 1;
            }
            return _requests.Max(l => l.Id) + 1;
        }
    }
}

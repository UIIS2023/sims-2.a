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
    public class WantedNewDateRepository : IWantedNewDateRepository
    {
        private const string FilePath = "../../../Resources/Data/wantedNewDates.csv";
        private readonly Serializer<WantedNewDate> _serializer;
        private List<WantedNewDate> _requests;

        public WantedNewDateRepository()
        {
            _serializer = new Serializer<WantedNewDate>();
            _requests = new List<WantedNewDate>();
        }

        public void Update(WantedNewDate wantedNewDate)
        {
            _requests = _serializer.FromCSV(FilePath);
            WantedNewDate current = _requests.Find(d => d.Id == wantedNewDate.Id) ?? throw new ArgumentException();
            int index = _requests.IndexOf(current);
            _requests.Remove(current);
            _requests.Insert(index, wantedNewDate);
            _serializer.ToCSV(FilePath, _requests);
        }

        public WantedNewDate GetById(int id)
        {
            _requests = _serializer.FromCSV(FilePath);
            return _requests.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public List<WantedNewDate> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(WantedNewDate wantedNewDate)
        {
            wantedNewDate.Id = NextId();
            _requests = _serializer.FromCSV(FilePath);
            _requests.Add(wantedNewDate);
            _serializer.ToCSV(FilePath, _requests);
        }

        public void Delete(int id)
        {
            _requests = _serializer.FromCSV(FilePath);
            WantedNewDate found = _requests.Find(t => t.Id == id) ?? throw new ArgumentException();
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

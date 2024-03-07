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
    public class TourRequestRepository : ITourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRequests.csv";
        private readonly Serializer<TourRequest> _serializer;
        private List<TourRequest> _requests;

        public TourRequestRepository()
        {
            _serializer = new Serializer<TourRequest>();
            _requests = new List<TourRequest>();
        }

        public void Update(TourRequest tourRequest)
        {
            _requests = _serializer.FromCSV(FilePath);
            TourRequest current = _requests.Find(d => d.Id == tourRequest.Id) ?? throw new ArgumentException();
            int index = _requests.IndexOf(current);
            _requests.Remove(current);
            _requests.Insert(index, tourRequest);
            _serializer.ToCSV(FilePath, _requests);
        }

        public TourRequest GetById(int id)
        {
            _requests = _serializer.FromCSV(FilePath);
            return _requests.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public List<TourRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(TourRequest tourRequest)
        {
            tourRequest.Id = NextId();
            _requests = _serializer.FromCSV(FilePath);
            _requests.Add(tourRequest);
            _serializer.ToCSV(FilePath, _requests);
        }

        public void Delete(int id)
        {
            _requests = _serializer.FromCSV(FilePath);
            TourRequest found = _requests.Find(t => t.Id == id) ?? throw new ArgumentException();
            _requests.Remove(found);
            _serializer.ToCSV(FilePath, _requests);
        }

        public List<TourRequest> GetInvalidRequests()
        {
            _requests = _serializer.FromCSV(FilePath);
            return _requests.FindAll(r => r.MaintenanceEndDate.AddDays(-2) < DateOnly.FromDateTime(DateTime.Now));
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

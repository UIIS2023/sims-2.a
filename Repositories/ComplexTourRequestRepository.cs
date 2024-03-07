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
    public class ComplexTourRequestRepository : IComplexTourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/ComplexTourRequests.csv";
        private readonly Serializer<ComplexTourRequest> _serializer;
        private List<ComplexTourRequest> _requests;

        public ComplexTourRequestRepository()
        {
            _serializer = new Serializer<ComplexTourRequest>();
            _requests = new List<ComplexTourRequest>();
        }

        public void Update(ComplexTourRequest tourRequest)
        {
            _requests = _serializer.FromCSV(FilePath);
            ComplexTourRequest current = _requests.Find(d => d.Id == tourRequest.Id) ?? throw new ArgumentException();
            int index = _requests.IndexOf(current);
            _requests.Remove(current);
            _requests.Insert(index, tourRequest);
            _serializer.ToCSV(FilePath, _requests);
        }

        public ComplexTourRequest GetById(int id)
        {
            _requests = _serializer.FromCSV(FilePath);
            return _requests.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public List<ComplexTourRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(ComplexTourRequest tourRequest)
        {
            tourRequest.Id = NextId();
            _requests = _serializer.FromCSV(FilePath);
            _requests.Add(tourRequest);
            _serializer.ToCSV(FilePath, _requests);
        }

        public void Delete(int id)
        {
            _requests = _serializer.FromCSV(FilePath);
            ComplexTourRequest found = _requests.Find(t => t.Id == id) ?? throw new ArgumentException();
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

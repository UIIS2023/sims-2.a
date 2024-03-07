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
    public class ChangedResRequestRepository : IChangedResRequestRepositroy
    {
        private const string FilePath = "../../../Resources/Data/changedResRequests.csv";
        private readonly Serializer<ChangedReservationRequest> _serializer;
        private List<ChangedReservationRequest> _requests;

        public ChangedResRequestRepository()
        {
            _serializer = new Serializer<ChangedReservationRequest>();
            _requests = new List<ChangedReservationRequest>();
        }

        public void Update(ChangedReservationRequest changeReservationRequest)
        {
            _requests = _serializer.FromCSV(FilePath);
            ChangedReservationRequest current = _requests.Find(d => d.Id == changeReservationRequest.Id) ?? throw new ArgumentException();
            int index = _requests.IndexOf(current);
            _requests.Remove(current);
            _requests.Insert(index, changeReservationRequest);
            _serializer.ToCSV(FilePath, _requests);
        }

        public ChangedReservationRequest GetById(int id)
        {
            _requests = _serializer.FromCSV(FilePath);
            return _requests.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public List<ChangedReservationRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(ChangedReservationRequest changeReservationRequest)
        {
            changeReservationRequest.Id = NextId();
            _requests = _serializer.FromCSV(FilePath);
            _requests.Add(changeReservationRequest);
            _serializer.ToCSV(FilePath, _requests);
        }

        public void Delete(int id)
        {
            _requests = _serializer.FromCSV(FilePath);
            ChangedReservationRequest found = _requests.Find(t => t.Id == id) ?? throw new ArgumentException();
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

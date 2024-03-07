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
    public class SuperGuestRepository : ISuperGuestRepository
    {
        private const string FilePath = "../../../Resources/Data/superGuest.csv";
        private readonly Serializer<SuperGuest> _serializer;
        private List<SuperGuest> _guests;

        public SuperGuestRepository()
        {
            _serializer = new Serializer<SuperGuest>();
            _guests = new List<SuperGuest>();
        }

        public void Update(SuperGuest superGuest)
        {
            _guests = _serializer.FromCSV(FilePath);
            SuperGuest current = _guests.Find(d => d.Id == superGuest.Id) ?? throw new ArgumentException();
            int index = _guests.IndexOf(current);
            _guests.Remove(current);
            _guests.Insert(index, superGuest);
            _serializer.ToCSV(FilePath, _guests);
        }

        public SuperGuest GetById(int id)
        {
            _guests = _serializer.FromCSV(FilePath);
            return _guests.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public List<SuperGuest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(SuperGuest superGuest)
        {
            superGuest.Id = NextId();
            _guests = _serializer.FromCSV(FilePath);
            _guests.Add(superGuest);
            _serializer.ToCSV(FilePath, _guests);
        }

        public void Delete(int id)
        {
            _guests = _serializer.FromCSV(FilePath);
            SuperGuest found = _guests.Find(t => t.Id == id) ?? throw new ArgumentException();
            _guests.Remove(found);
            _serializer.ToCSV(FilePath, _guests);
        }

        public int NextId()
        {
            _guests = _serializer.FromCSV(FilePath);
            if (_guests.Count < 1)
            {
                return 1;
            }
            return _guests.Max(l => l.Id) + 1;
        }
    }
}

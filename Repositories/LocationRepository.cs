using System;
using System.Collections.Generic;
using System.Linq;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private const string FilePath = "../../../Resources/Data/locations.csv";

        private readonly Serializer<Location> _serializer;

        private List<Location> _locations;

        public LocationRepository()
        {
            _serializer = new Serializer<Location>();
            _locations = _serializer.FromCSV(FilePath);
        }

        public void Delete(int id)
        {
            _locations = _serializer.FromCSV(FilePath);
            Location founded = _locations.Find(l => l.Id == id) ?? throw new ArgumentException();
            _locations.Remove(founded);
            _serializer.ToCSV(FilePath, _locations);
        }

        public List<Location> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Location? GetById(int id)
        {
            _locations = _serializer.FromCSV(FilePath);
            return _locations.Find(l => l.Id == id);
        }

        public int NextId()
        {
            _locations = _serializer.FromCSV(FilePath);
            if (_locations.Count < 1)
            {
                return 1;
            }
            return _locations.Max(l => l.Id) + 1;
        }

        public void Save(Location location)
        {
            location.Id = NextId();
            _locations = _serializer.FromCSV(FilePath);
            _locations.Add(location);
            _serializer.ToCSV(FilePath, _locations);
        }

        public void Update(Location location)
        {
            _locations = _serializer.FromCSV(FilePath);
            Location current = _locations.Find(l => l.Id == location.Id) ?? throw new ArgumentException();
            int index = _locations.IndexOf(current);
            _locations.Remove(current);
            _locations.Insert(index, location);
            _serializer.ToCSV(FilePath, _locations);
        }
    }
}

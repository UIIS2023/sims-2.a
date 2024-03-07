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
    public class AccommodationRenovationRepository : IAccommodationRenovationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRenovation.csv";
        private readonly Serializer<AccommodationRenovation> _serializer;
        private List<AccommodationRenovation> _accommodationRenovations;

        public AccommodationRenovationRepository()
        {
            _serializer = new Serializer<AccommodationRenovation>();
            _accommodationRenovations = new List<AccommodationRenovation>();
        }

        public List<AccommodationRenovation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(AccommodationRenovation accommodationRenovation)
        {
            accommodationRenovation.Id = NextId();
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            _accommodationRenovations.Add(accommodationRenovation);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
        }

        public void Delete(int id)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            AccommodationRenovation found = _accommodationRenovations.Find(t => t.Id == id) ?? throw new ArgumentException();
            _accommodationRenovations.Remove(found);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
        }

        public void Update(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            AccommodationRenovation current = _accommodationRenovations.Find(t => t.Id == accommodationRenovation.Id) ?? throw new ArgumentException();
            int index = _accommodationRenovations.IndexOf(current);
            _accommodationRenovations.Remove(current);
            _accommodationRenovations.Insert(index, accommodationRenovation);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
        }

        public int NextId()
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            if (_accommodationRenovations.Count < 1)
            {
                return 1;
            }
            return _accommodationRenovations.Max(l => l.Id) + 1;
        }

        public AccommodationRenovation GetById(int id)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            return _accommodationRenovations.Find(l => l.Id == id) ?? throw new ArgumentException();
        }
    }

}

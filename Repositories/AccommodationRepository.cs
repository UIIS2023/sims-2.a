using System;
using System.Collections.Generic;
using System.Linq;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodation.csv";
        private readonly Serializer<Accommodation> _serializer;
        private List<Accommodation> _accommodations;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = new List<Accommodation>();
        }

        public List<Accommodation> GetAll() 
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(Accommodation accommodation) 
        {
            accommodation.Id = NextId();
            _accommodations = _serializer.FromCSV(FilePath);
            _accommodations.Add(accommodation);
            _serializer.ToCSV(FilePath,_accommodations);
        }

        public void Delete(int id)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation found = _accommodations.Find(t => t.Id == id) ?? throw new ArgumentException();
            _accommodations.Remove(found);
            _serializer.ToCSV(FilePath, _accommodations);
        }

        public void Update(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation current = _accommodations.Find(t => t.Id == accommodation.Id) ?? throw new ArgumentException();
            int index = _accommodations.IndexOf(current);
            _accommodations.Remove(current);
            _accommodations.Insert(index, accommodation);
            _serializer.ToCSV(FilePath, _accommodations);
        }
        public int NextId()
        {
            _accommodations = _serializer.FromCSV(FilePath);
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(l => l.Id) + 1;
        }

        public Accommodation GetById(int id)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            return _accommodations.Find(t => t.Id == id) ?? throw new ArgumentException();
        }

        public List<Accommodation> GetAllByUserId(int id)
        {
            return GetAll().Where(p => p.OwnerId == id).ToList();
        }

        public Accommodation SaveAndReturn(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations = _serializer.FromCSV(FilePath);
            _accommodations.Add(accommodation);
            _serializer.ToCSV(FilePath, _accommodations);
            return accommodation;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class TourRepository : ITourRepository
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";

        private readonly Serializer<Tour> _serializer;

        private List<Tour> _tours;

        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = new List<Tour>();
        }

        public void Delete(int id)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour founded = _tours.Find(t => t.Id == id) ?? throw new ArgumentException();
            _tours.Remove(founded);
            _serializer.ToCSV(FilePath, _tours);
        }

        public List<Tour> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Tour? GetById(int id)
        {
            _tours = _serializer.FromCSV(FilePath);
            return _tours.Find(t => t.Id == id);
        }

        public int NextId()
        {
            _tours = _serializer.FromCSV(FilePath);
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(t => t.Id) + 1;
        }

        public void Save(Tour tour)
        {
            tour.Id = NextId();
            _tours = _serializer.FromCSV(FilePath);
            _tours.Add(tour);
            _serializer.ToCSV(FilePath, _tours);
        }
        public void Update(Tour image)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour current = _tours.Find(t => t.Id == image.Id) ?? throw new ArgumentException();
            int index = _tours.IndexOf(current);
            _tours.Remove(current);
            _tours.Insert(index, image);
            _serializer.ToCSV(FilePath, _tours);
        }
    }
}

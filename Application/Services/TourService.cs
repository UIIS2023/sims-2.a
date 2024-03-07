using System.Collections.Generic;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class TourService
    {

        private readonly ITourRepository _tourRepository;
        private readonly LocationService _locationService;

        public TourService()
        {
            _tourRepository = Injector.CreateInstance<ITourRepository>();
            _locationService = new LocationService();
        }
        public void Delete(int id)
        {
            _tourRepository.Delete(id);
        }

        public List<Tour> GetAll()
        {
            return _tourRepository.GetAll();
        }

        public Tour? GetById(int id)
        {
            return _tourRepository.GetById(id);
        }

        public void Save(Tour tour)
        {
            _tourRepository.Save(tour);
        }

        public void Update(Tour tour)
        {
            _tourRepository.Update(tour);
        }

        public int NextId()
        {
            return _tourRepository.NextId();
        }

        public Tour? FindTourById(int id)
        {
            foreach(var tour in _tourRepository.GetAll())
            {
                if(tour.Id == id) return tour;
            }
            return null;
        }

        public List<Tour> GetSameLocatedTours(int locationId)
        {
            List<Tour> tours = new List<Tour>();
            foreach(Tour tour in _tourRepository.GetAll())
            {
                if(tour.LocationId == locationId)
                {
                    tours.Add(tour);
                }
            }
            return tours;
        }
        public string GetTourCity(Tour tour)
        {
            foreach(Location location in _locationService.GetAll())
            {
                if(tour.LocationId == location.Id)
                {
                    return location.City;
                }
            }
            return string.Empty;
        }
        public string GetTourCountry(Tour tour)
        {
            foreach (Location location in _locationService.GetAll())
            {
                if (tour.LocationId == location.Id)
                {
                    return location.Country;
                }
            }
            return string.Empty;
        }
        public string GetTourName(int id)
        {
            foreach(Tour tour in _tourRepository.GetAll())
            {
                if(tour.Id == id)
                {
                    return tour.Name;
                }
            }
            return string.Empty;
        }
    }
}

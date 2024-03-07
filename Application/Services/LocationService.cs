using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class LocationService
    {
        private readonly ILocationRepository _locationRepository = Injector.CreateInstance<ILocationRepository>();
        public LocationService() { }

        public void Delete(int id)
        {
            _locationRepository.Delete(id);
        }

        public List<Location> GetAll()
        {
            return _locationRepository.GetAll();
        }

        public Location? GetById(int id)
        {
            return _locationRepository.GetById(id);
        }

        public void Save(Location location)
        {
            _locationRepository.Save(location);
        }

        public void Update(Location location)
        {
            _locationRepository.Update(location);
        }

        public string GetFullName(Location location)
        {
            string fullName = location.City.ToString();
            fullName += " (";
            fullName += location.Country.ToString();
            fullName += ")";
            return fullName;
        }

    }
}

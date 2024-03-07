using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;


namespace TravelAgency.Converter
{

    public class LocationConverter
    {

        private LocationRepository _locationRepository;
        public LocationConverter()
        {
            _locationRepository = new();
        }

        public List<string> GetFullNames()
        {
            List<Location> locations = _locationRepository.GetAll();
            List<string> fullNames = new List<string>();
            foreach (Location location in locations)
            {
                string fullName = location.City + " (" + location.Country + ")";
                fullNames.Add(fullName);
            }

            return fullNames;
        }

        public string GetFullNameById(int locationId)
        {
            Location location = _locationRepository.GetById(locationId);
            string fullName = location.City + " (" + location.Country + ")";

            return fullName;
        }
    }
}

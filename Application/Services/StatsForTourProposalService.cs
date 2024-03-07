using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class StatsForTourProposalService
    {
        private readonly TourRequestService _tourRequestService;

        public StatsForTourProposalService()
        {
            _tourRequestService = new TourRequestService();
        }

        public Location GetMostRequestedLocation()
        {
            var numOfRequestsPerLocation = new Dictionary<Location, int>();

            foreach (var location in GetDistinctLocations())
            {
                int numOfRequests = 0;
                foreach (var requestLocation in GetAllLocationsFormRequests())
                {
                    if (location.City == requestLocation.City && location.Country == requestLocation.Country)
                    {
                        numOfRequests++;
                    }
                }
                numOfRequestsPerLocation.Add(location, numOfRequests);
            }

            var sortedDictionary = numOfRequestsPerLocation.OrderBy(x => x.Value);

            return sortedDictionary.LastOrDefault().Key;
        }


        private List<Location> GetAllLocationsFormRequests()
        {
            var locations = new List<Location>();

            foreach (var request in _tourRequestService.GetAllInLastYear())
            {
                var location = new Location
                {
                    City = request.City,
                    Country = request.Country
                };
                locations.Add(location);
            }

            return locations;
        }

        private List<Location> GetDistinctLocations()
        {
            var locations = new List<Location>();

            foreach (var request in _tourRequestService.GetAllInLastYear())
            {
                var location = new Location
                {
                    Id = -1,
                    City = request.City,
                    Country = request.Country,
                };
                if (!IsLocationAlreadyExists(locations, location))
                {
                    locations.Add(location);
                }
            }

            var locationsDistinct = new List<Location>(locations.Distinct());

            return locationsDistinct;
        }

        private bool IsLocationAlreadyExists(List<Location> locations, Location location)
        {
            return locations.Any(l => l.City == location.City && l.Country == location.Country);
        }

        public string GetMostRequiredLanguage()
        {
            var numOfRequestsPerLanguage = new Dictionary<string, int>();

            foreach (var language in GetAllRequiredLanguagesDistinct())
            {
                int numOfRequests = 0;
                foreach (var requiredLanguage in GetAllRequiredLanguages())
                {
                    if (language == requiredLanguage)
                    {
                        numOfRequests++;
                    }
                }
                numOfRequestsPerLanguage.Add(language, numOfRequests);
            }

            var sortedDictionary = numOfRequestsPerLanguage.OrderBy(x => x.Value);

            return sortedDictionary.LastOrDefault().Key;
        }

        private List<string> GetAllRequiredLanguages()
        {
            var languages = new List<string>();

            foreach (var request in _tourRequestService.GetAllInLastYear())
            {
                languages.Add(request.Language);
            }

            return languages;
        }

        private List<string> GetAllRequiredLanguagesDistinct()
        {
            var distinctLanguages = new List<string>();

            foreach (var request in _tourRequestService.GetAllInLastYear())
            {
                if (!IsLanguageAlreadyExists(distinctLanguages, request.Language))
                {
                    distinctLanguages.Add(request.Language);
                }
            }

            return distinctLanguages;
        }


        private bool IsLanguageAlreadyExists(List<string> languages, string language)
        {
            return languages.Any(l => l == language);
        }

    }
}

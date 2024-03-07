using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class TourRequestStatsService
    {
        private readonly TourRequestService _tourRequestService;

        public TourRequestStatsService()
        {
            _tourRequestService = new TourRequestService();
        }

        public ObservableCollection<int> GetAvailableYears()
        {
            ObservableCollection<int> availableYears = new ObservableCollection<int>();

            foreach (var request in _tourRequestService.GetAll())
            {
                availableYears.Add(request.MaintenanceStartDate.Year);
                availableYears.Add(request.MaintenanceEndDate.Year);
            }

            var distinctYears = availableYears.Distinct().ToList();

            availableYears.Clear();

            foreach (var year in distinctYears)
            {
                availableYears.Add(year);
            }
            return availableYears;
        }

        public Dictionary<string, int> GenerateTourRequestsByLocation(User loggedInUser)
        {
            Dictionary<string, int> locationCounts = new Dictionary<string, int>();

            foreach (var request in _tourRequestService.GetAll())
            {
                if (request.UserId == loggedInUser.Id)
                {
                    string location = request.City + ", " + request.Country;

                    if (locationCounts.ContainsKey(location))
                        locationCounts[location]++;
                    else
                        locationCounts[location] = 1;
                }
            }

            return locationCounts;
        }

        public Dictionary<string, int> GenerateTourRequestsByLanguage(User loggedInUser)
        {
            Dictionary<string, int> languageCounts = new Dictionary<string, int>();

            foreach (var request in _tourRequestService.GetAll())
            {
                if (request.UserId == loggedInUser.Id)
                {
                    string language = request.Language;

                    if (languageCounts.ContainsKey(language))
                        languageCounts[language]++;
                    else
                        languageCounts[language] = 1;
                }
            }

            return languageCounts;
        }

        public (float,float,float) GenerateStatistics(User loggedInUser,int selectedYear = 0)
        {
            (float,float,float) statsData = (0, 0, 0);
            int acceptedRequests = 0;
            int invalidRequests = 0;
            int totalTouristNum = 0;
            int userRequestsNum = 0;

            foreach (var request in _tourRequestService.GetAll())
            {
                if (request.UserId == loggedInUser.Id && (selectedYear == 0 || request.MaintenanceStartDate.Year == selectedYear || request.MaintenanceEndDate.Year == selectedYear))
                {
                    if (request.Status == StatusType.ACCEPTED)
                    {
                        acceptedRequests++;
                        totalTouristNum += request.MaxNumOfGuests;
                    }
                    else if (request.Status == StatusType.INVALID)
                    {
                        invalidRequests++;
                    }

                    userRequestsNum++;
                }
            }


                statsData.Item1 = acceptedRequests == 0 ? 0 : totalTouristNum / acceptedRequests;
                statsData.Item2 = userRequestsNum == 0 ? 0 : (float)acceptedRequests / userRequestsNum * 100;
                statsData.Item3 = userRequestsNum == 0 ? 0 : (float)invalidRequests / userRequestsNum * 100;
                return statsData;
        }
    }


}

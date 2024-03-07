using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class SearchForTourRequestStats
    {
        private readonly TourRequestService _tourRequestService;

        public SearchForTourRequestStats()
        {
            _tourRequestService = new TourRequestService();
        }

        public ObservableCollection<NumOfTourRequestsPerYearViewModel> GetStatsByLanguage(string? language)
        {
            if (language == null)
            {
                return new ObservableCollection<NumOfTourRequestsPerYearViewModel>();
            }

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("ar-EG");
            var numOfRequestPerYearAndMonth = new List<NumOfTourRequestsPerYearViewModel>();
            foreach (var year in GetAvailableYearsByLanguage(language))
            {
                var numOfTourRequests = new NumOfTourRequestsPerYearViewModel(year, 0);
                
                foreach (var request in _tourRequestService.GetAllByLanguage(language))
                {
                    if (request.CreationTime.Year.ToString() == year)
                    {
                        foreach (var numOfRequestsPerMonth in numOfTourRequests.NumOfRequestsPerMonths)
                        {
                            if (numOfRequestsPerMonth.MonthConverter() == request.CreationTime.Month.ToString())
                            {
                                numOfRequestsPerMonth.NumOfRequestsPerMonth++;
                            }
                        }
                        numOfTourRequests.NumOfRequestsPerYear++;
                    }
                }
                numOfRequestPerYearAndMonth.Add(numOfTourRequests);
            }

            return new ObservableCollection<NumOfTourRequestsPerYearViewModel>(numOfRequestPerYearAndMonth);
        }

        private List<string> GetAvailableYearsByLanguage(string language)
        {
            var availableYears = new List<string>();
            foreach (var request in _tourRequestService.GetAllByLanguage(language))
            {
                availableYears.Add(request.CreationTime.Year.ToString());
            }

            var distinctAvailableYears = new List<string>(availableYears.Distinct());

            return distinctAvailableYears;
        }


        public ObservableCollection<NumOfTourRequestsPerYearViewModel> GetStatsByLocation(string? city, string country)
        {
            var numOfRequestsPerYearAndMonths = new List<NumOfTourRequestsPerYearViewModel>();
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("ar-EG");

            if (city == null)
            {
                return new ObservableCollection<NumOfTourRequestsPerYearViewModel>(numOfRequestsPerYearAndMonths);
            }

            foreach (var year in GetAvailableYearsByLocation(city, country))
            {
                var numOfTourRequests = new NumOfTourRequestsPerYearViewModel(year, 0);

                foreach (var request in _tourRequestService.GetAllByLocation(city, country))
                {
                    if (request.CreationTime.Year.ToString() == year)
                    {
                        foreach (var numOfRequestsPerMonth in numOfTourRequests.NumOfRequestsPerMonths)
                        {
                            if (numOfRequestsPerMonth.MonthConverter() == request.CreationTime.Month.ToString())
                            {
                                numOfRequestsPerMonth.NumOfRequestsPerMonth++;
                            }
                        }
                        numOfTourRequests.NumOfRequestsPerYear++;
                    }
                }
                numOfRequestsPerYearAndMonths.Add(numOfTourRequests);
            }
            return new ObservableCollection<NumOfTourRequestsPerYearViewModel>(numOfRequestsPerYearAndMonths);
        }


        private List<string> GetAvailableYearsByLocation(string city, string country)
        {
            var availableYears = new List<string>();
            foreach (var request in _tourRequestService.GetAllByLocation(city, country))
            {
                availableYears.Add(request.CreationTime.Year.ToString());
            }

            var distinctAvailableYears = new List<string>(availableYears.Distinct());

            return distinctAvailableYears;
        }

    }
}

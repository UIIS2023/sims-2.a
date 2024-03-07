using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Creators;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class TourRequestSearchViewModel
    {
        private readonly TourRequestService _tourRequestService;

        public TourRequestSearchViewModel()
        {
            _tourRequestService = new TourRequestService();
        }


        public ObservableCollection<TourRequestCardViewModel> SearchTourRequests(string city, string country,
                                                                                 int? numOfGuests, string language,
                                                                                 DateTime? minDate, DateTime? maxDate)
        {
            var searchDateRange = new DateRange(minDate, maxDate);

            var filteredRequests = _tourRequestService.GetAllRegularOnHold().Where(request =>
                (string.IsNullOrEmpty(city) || request.City == city) &&
                (string.IsNullOrEmpty(country) || request.Country == country) &&
                (!numOfGuests.HasValue || request.MaxNumOfGuests == numOfGuests) &&
                (string.IsNullOrEmpty(language) || request.Language == language) &&
                ((!minDate.HasValue && !maxDate.HasValue) || !FindRequestDateRange(request).IsOutOfRange(searchDateRange))).ToList();


            return CreateTourRequestCards(filteredRequests);


        }


        private ObservableCollection<TourRequestCardViewModel> CreateTourRequestCards(
            List<TourRequest> filteredRequests)
        {
            var tourRequestCards = new ObservableCollection<TourRequestCardViewModel>();
            foreach (var request in filteredRequests)
            {
                var tourRequestCard = new TourRequestCardViewModel
                {
                    Id = request.Id,
                    Language = request.Language,
                    NumOfGuests = request.MaxNumOfGuests,
                    Location = request.Country + ", " + request.City,
                    MinDate = new DateTime(request.MaintenanceStartDate.Year, request.MaintenanceStartDate.Month, request.MaintenanceStartDate.Day),
                    MaxDate = new DateTime(request.MaintenanceEndDate.Year, request.MaintenanceEndDate.Month, request.MaintenanceEndDate.Day)
                };
                tourRequestCards.Add(tourRequestCard);
            }
            return tourRequestCards;
        }

        private DateRange FindRequestDateRange(TourRequest tourRequest)
        {
            var maintenanceStartDate = new DateTime(tourRequest.MaintenanceStartDate.Year,
                tourRequest.MaintenanceStartDate.Month, tourRequest.MaintenanceStartDate.Day);

            var maintenanceEndDate = new DateTime(tourRequest.MaintenanceEndDate.Year,
                tourRequest.MaintenanceEndDate.Month, tourRequest.MaintenanceEndDate.Day);

            var requestDateRange = new DateRange(maintenanceStartDate, maintenanceEndDate);

            return requestDateRange;
        }
    }
}

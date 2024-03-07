using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;

namespace SOSTeam.TravelAgency.WPF.Creators
{
    public class TourRequestCardCreatorViewModel
    {
        private readonly TourRequestService _tourRequestService;

        public TourRequestCardCreatorViewModel()
        {
            _tourRequestService = new TourRequestService();
            _tourRequestService.UpdateInvalidRequests();
        }

        public ObservableCollection<TourRequestCardViewModel> CreateRegularTourRequestCards()
        {
            var tourRequestCards = new ObservableCollection<TourRequestCardViewModel>();

            foreach (var tourRequest in _tourRequestService.GetAllRegularOnHold())
            {
                var tourRequestCard = new TourRequestCardViewModel
                {
                    Id = tourRequest.Id,
                    Language = tourRequest.Language,
                    NumOfGuests = tourRequest.MaxNumOfGuests,
                    Location = tourRequest.Country + ", " + tourRequest.City,
                    MinDate = new DateTime(tourRequest.MaintenanceStartDate.Year, tourRequest.MaintenanceStartDate.Month, tourRequest.MaintenanceStartDate.Day),
                    MaxDate = new DateTime(tourRequest.MaintenanceEndDate.Year, tourRequest.MaintenanceEndDate.Month, tourRequest.MaintenanceEndDate.Day)
                };
                tourRequestCards.Add(tourRequestCard);
            }

            return tourRequestCards;
        }

        public ObservableCollection<TourRequestCardViewModel> CreateComplexTourRequestCards(int complexTourId)
        {
            var tourRequestCards = new ObservableCollection<TourRequestCardViewModel>();

            foreach (var tourRequest in _tourRequestService.GetAllComplexRequestsOnHold(complexTourId))
            {
                var tourRequestCard = new TourRequestCardViewModel
                {
                    Id = tourRequest.Id,
                    Language = tourRequest.Language,
                    NumOfGuests = tourRequest.MaxNumOfGuests,
                    Location = tourRequest.Country + ", " + tourRequest.City,
                    MinDate = new DateTime(tourRequest.MaintenanceStartDate.Year, tourRequest.MaintenanceStartDate.Month, tourRequest.MaintenanceStartDate.Day),
                    MaxDate = new DateTime(tourRequest.MaintenanceEndDate.Year, tourRequest.MaintenanceEndDate.Month, tourRequest.MaintenanceEndDate.Day)
                };
                tourRequestCards.Add(tourRequestCard);
            }

            return tourRequestCards;
        }

    }
}

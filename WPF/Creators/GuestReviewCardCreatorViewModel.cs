using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestReviewCardCreatorViewModel
    {
        private readonly TourService _tourService;
        private readonly UserService _userService;
        private readonly TourReviewService _tourReviewService;

        public GuestReviewCardCreatorViewModel()
        {
            _tourService = new TourService();
            _userService = new UserService();
            _tourReviewService = new TourReviewService();
        }

        public ObservableCollection<GuestReviewCardViewModel> CreateCards(TourCardViewModel selectedTour)
        {
            var guestReviewCards = new ObservableCollection<GuestReviewCardViewModel>();
            foreach (var tourReview in _tourReviewService.GetAllByAppointmentId(selectedTour.AppointmentId))
            {
                var guestReviewCard = CreateCard(selectedTour, tourReview);

                guestReviewCards.Add(guestReviewCard);
            }

            return guestReviewCards;
        }

        private GuestReviewCardViewModel CreateCard(TourCardViewModel selectedTour, TourReview tourReview)
        {
            var guestReviewCard = new GuestReviewCardViewModel
            {
                ReviewId = tourReview.Id,
                AppointmentId = selectedTour.AppointmentId,
                UserId = tourReview.UserId,
                Date = selectedTour.Start,
                AvgGrade = Math.Round(FindAvgGrade(tourReview), 1) + "/5.0",
                KnowledgeGrade = tourReview.GuideKnowledge,
                LanguageGrade = tourReview.GuideLanguage,
                InterestingGrade = tourReview.InterestRating,
                Comment = tourReview.Comment,
                GuestName = _userService.GetById(tourReview.UserId).Username,
                TourName = _tourService.GetById(selectedTour.TourId).Name,
            };
            if (tourReview.Reported)
            {
                guestReviewCard.SetReportedImage();
            }

            return guestReviewCard;
        }

        private double FindAvgGrade(TourReview tourReview)
        {
            return (double)(tourReview.GuideKnowledge + tourReview.GuideLanguage + tourReview.InterestRating) / 3;
        }

    }
}

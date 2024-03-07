using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    internal class GuestReviewPageViewModel : ViewModel
    {
        private GuestReviewService _guestReviewService;
        private AccommodationService _accommodationService;
        private UserService _userService;

        public User LoggedInUser { get; private set; }
        public User GuestUser { get; private set; }
        public Accommodation Accommodation { get; private set; }
        public GuestReview Review { get; private set; }
        public int CleanlinessRating { get; set; }
        public int RulesRating { get; set; }
        public string Comment { get; set; }
        public RelayCommand Cancel { get; private set; }
        public RelayCommand AddReview { get; private set; }
        public RelayCommand RateCleanliness { get; private set; }
        public RelayCommand RateRules { get; private set; }
        public GuestReviewPageViewModel(int reviewId)
        {
            LoggedInUser = App.LoggedUser;
            _guestReviewService = new();
            _userService = new();
            _accommodationService = new();
            Review = _guestReviewService.GetById(reviewId);
            GuestUser = _userService.GetById(Review.GuestId);
            Accommodation = _accommodationService.GetById(Review.AccommodationId);
            CleanlinessRating = 5;
            RulesRating = 5;
            AddReview = new RelayCommand(Execute_AddReview, CanExecuteAddReview);
            Cancel = new RelayCommand(Execute_Cancel, CanExecuteCancel);
            RateCleanliness = new RelayCommand(Execute_RateCleanliness, CanExecuteCancel);
            RateRules = new RelayCommand(Execute_RateRules, CanExecuteCancel);

        }


        private void Execute_RateCleanliness(object obj)
        {
            CleanlinessRating = System.Convert.ToInt32(obj);
            OnPropertyChanged("CleanlinessRating");
        }

        private void Execute_RateRules(object obj)
        {
            RulesRating = System.Convert.ToInt32(obj);
            OnPropertyChanged("RulesRating");
        }

        private bool CanExecuteAddReview(object obj)
        {
            return true;
        }
        private bool CanExecuteCancel(object obj)
        {
            return true;
        }

        private void Execute_AddReview(object obj)
        {
            if (Comment == null) Comment = "";
            Review.IsReviewed = true;
            Review.CleanlinessGrade = CleanlinessRating;
            Review.RespectGrade = RulesRating;
            Review.Comment = Comment;
            _guestReviewService.Update(Review);


            App.OwnerNavigationService.NavigateMainWindow("Home");
            return;
        }

        private void Execute_Cancel(object obj)
        {
            App.OwnerNavigationService.NavigateMainWindow("Home");
            return;
        }


    }

    public class RatingToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is int rating && rating > 0)
            {
                int starRating = System.Convert.ToInt32(parameter);
                return starRating <= rating ? Brushes.Gold : Brushes.Gray;
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is int intValue)
            {
                return intValue == int.Parse(parameter?.ToString() ?? "0");
            }
            return false;
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is bool isChecked && isChecked)
            {
                return int.Parse(parameter?.ToString() ?? "0");
            }
            return Binding.DoNothing;
        }

    }
}

using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class SuggestionPageViewModel : ViewModel
    {
        private AccommodationService _accommodationService;


        public RenovationRecommendationViewModel SelectedRecommendation { get; set; }
        public Accommodation PopularAccommodation { get; set; }
        public Accommodation UnpopularAccommodation { get; set; }
        public string PopularAccommodationURI { get; set; }
        public string UnpopularAccommodationURI { get; set; }
        public string UnpopularLocation { get; set; }
        public string PopularLocation { get; set; }
        public RelayCommand DeleteRecommendation { get; private set; }



        private ObservableCollection<RenovationRecommendationViewModel> _renovationRecommendations;
        public ObservableCollection<RenovationRecommendationViewModel> RenovationRecommendations
        {
            get { return _renovationRecommendations; }
            set
            {
                _renovationRecommendations = value;
                OnPropertyChanged("RenovationRecommendations");
            }
        }
        private RenovationRecommendationService _renovationRecommendationService;
        private UserService _userService;
        private AccommodationStatsService _accommodationStatsService;
        private ImageService _imageService;
        private LocationService _locationService;

        public SuggestionPageViewModel()
        {
            _renovationRecommendationService = new RenovationRecommendationService();
            RenovationRecommendations = new();
            _userService = new();
            _accommodationService = new();
            _accommodationStatsService = new(App.LoggedUser.Id);
            _imageService = new();
            _locationService = new();
            DeleteRecommendation = new RelayCommand(Execute_DeleteRecommendation, CanExecuteDeleteRecommendation);
            FillObservableCollection();
            GetMostPopularAndUnpopularAccommodation();

        }

        private void FillObservableCollection()
        {
            var a = _renovationRecommendationService.GetAllForUser(App.LoggedUser.Id);
            foreach (var recommendation in a)
            {
                RenovationRecommendations.Add(new RenovationRecommendationViewModel(
                                              recommendation.Id,
                                              _userService.GetById(recommendation.GuestId).Username,
                                              recommendation.RenovationRank,
                                              recommendation.Comment,
                                              _accommodationService.GetById(recommendation.AccommodationId).Name
                    ));
            }
        }

        private bool CanExecuteDeleteRecommendation(object obj)
        {
            return SelectedRecommendation != null;
        }

        private void Execute_DeleteRecommendation(object obj)
        {
            _renovationRecommendationService.Delete(SelectedRecommendation.Id);
            RenovationRecommendations.Remove(SelectedRecommendation);
        }

        private void GetMostPopularAndUnpopularAccommodation()
        {

            PopularAccommodation = _accommodationStatsService.GetMostPopularAccommodation();
            var photo = _imageService.GetAccommodationCover(PopularAccommodation.Id);
            PopularAccommodationURI = "/Resources/Images/UnknownPhoto.png";
            if (photo != null) PopularAccommodationURI = photo.Path;
            PopularLocation = _locationService.GetFullName(_locationService.GetById(PopularAccommodation.LocationId));
            
            UnpopularAccommodation = _accommodationStatsService.GetMostUnpopularAccommodation();
            photo = _imageService.GetAccommodationCover(UnpopularAccommodation.Id);
            UnpopularAccommodationURI = "/Resources/Images/UnknownPhoto.png";
            if (photo != null) UnpopularAccommodationURI = photo.Path;
            UnpopularLocation = _locationService.GetFullName(_locationService.GetById(UnpopularAccommodation.LocationId));

        }


    }

    public class RenovationRecommendationViewModel
    {
        public int Id { get; set; }
        public string Accommodation { get; set; }
        public string Username { get; set; }
        public int RenovationRank { get; set; }
        public string Comment { get; set; }

        public RenovationRecommendationViewModel(int id, string username, int renovationRank, string comment, string accommodation)
        {
            Id = id;
            Username = username;
            RenovationRank = renovationRank;
            Comment = comment;
            Accommodation = accommodation;
        }
    }


}

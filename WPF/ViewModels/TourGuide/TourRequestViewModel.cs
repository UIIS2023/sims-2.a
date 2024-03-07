using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Creators;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{

    public class TourRequestViewModel : ViewModel
    {
        private string _city;

        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged("City");
                    SearchTourRequests();
                }
            }
        }

        private string _country;

        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged("Country");
                    SearchTourRequests();
                }
            }
        }

        private int? _numOfGuests;

        public int? NumOfGuests
        {
            get => _numOfGuests;
            set
            {
                if (_numOfGuests != value)
                {
                    _numOfGuests = value;
                    OnPropertyChanged("NumOfGuests");
                    SearchTourRequests();
                }
            }
        }

        private string _language;

        public string Language
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged("Language");
                    SearchTourRequests();
                }
            }
        }

        private DateTime? _minDate;

        public DateTime? MinDate
        {
            get => _minDate;
            set
            {
                if (_minDate != value)
                {
                    _minDate = value;
                    OnPropertyChanged("MinDate");
                    SearchTourRequests();
                }
            }
        }

        private DateTime? _maxDate;

        public DateTime? MaxDate
        {
            get => _maxDate;
            set
            {
                if (_maxDate != value)
                {
                    _maxDate = value;
                    OnPropertyChanged("MaxDate");
                    SearchTourRequests();
                }
            }
        }

        private ObservableCollection<TourRequestCardViewModel> _tourRequestCards;

        public ObservableCollection<TourRequestCardViewModel> TourRequestCards
        {
            get => _tourRequestCards;
            set
            {
                if (_tourRequestCards != value)
                {
                    _tourRequestCards = value;
                    OnPropertyChanged("TourRequestCards");
                }
            }
        }

        public ObservableCollection<string> Languages { get; set; }

        public ObservableCollection<string> Countries { get; set; }


        private ObservableCollection<string> _cities;

        public ObservableCollection<string> Cities
        {
            get => _cities;
            set
            {
                if (_cities != value)
                {
                    _cities = value;
                    OnPropertyChanged("Cities");
                }
            }
        }

        private readonly List<Location> _locations;

        private readonly TourRequestService _tourRequestService;

        private readonly TourRequestSearchViewModel _tourRequestSearch;

        public TourRequestCardViewModel SelectedTourRequestCard { get; set; }

        public RelayCommand CitySelectionChangedCommand { get; set; }
        public RelayCommand CountrySelectionChangedCommand { get; set; }
        public RelayCommand ResetFormCommand { get; set; }
        public RelayCommand ShowAcceptTourRequestFormCommand { get; set; }



        public TourRequestViewModel()
        {
            _tourRequestService = new TourRequestService();
            _tourRequestSearch = new TourRequestSearchViewModel();
            var tourRequestCardCreator = new TourRequestCardCreatorViewModel();
            _tourRequestCards = tourRequestCardCreator.CreateRegularTourRequestCards();

            Languages = GetLanguages();
            Countries = GetCountries();
            _cities = GetCities();
            _locations = GetLocations();
            
            _city = string.Empty;
            _country = string.Empty;
            _language = string.Empty;
            _numOfGuests = null;
            _maxDate = null;
            _maxDate = null;
            

            CountrySelectionChangedCommand = new RelayCommand(ExecuteCountrySelectionChanged, CanExecuteMethod);
            CitySelectionChangedCommand = new RelayCommand(ExecuteCitySelectionChanged, CanExecuteMethod);
            ResetFormCommand = new RelayCommand(ResetForm, CanExecuteMethod);
            ShowAcceptTourRequestFormCommand = new RelayCommand(ShowAcceptTourRequestForm, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public void ExecuteCountrySelectionChanged(object parameter)
        {
            CountrySelectionChanged(parameter, null);
        }

        public void ExecuteCitySelectionChanged(object parameter)
        {
            CitySelectionChanged(parameter, null);
        }

        public void CountrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cities.Clear();

            var filteredLocations = _locations.Where(location => location.Country == Country).ToList();

            foreach (var location in filteredLocations)
            {
                Cities.Add(location.City);
            }
        }

        public void CitySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedLocation = _locations.Find(location => location.City == City);

            if (selectedLocation != null)
            {
                Country = selectedLocation.Country;
                City = selectedLocation.City;
            }
        }

        private ObservableCollection<string> GetLanguages()
        {
            var languages = new ObservableCollection<string>();

            foreach (var request in _tourRequestService.GetAllRegularOnHold())
            {
                languages.Add(request.Language);
            }

            var languagesDistinct = new ObservableCollection<string>(languages.Distinct());

            return languagesDistinct;
        }

        private ObservableCollection<string> GetCities()
        {
            var cities = new ObservableCollection<string>();
            foreach (var request in _tourRequestService.GetAllRegularOnHold())
            {
                cities.Add(request.City);
            }

            var citiesDistinct = new ObservableCollection<string>(cities.Distinct());

            return citiesDistinct;
        }

        private ObservableCollection<string> GetCountries()
        {
            var countries = new ObservableCollection<string>();
            foreach (var request in _tourRequestService.GetAllRegularOnHold())
            {
                countries.Add(request.Country);
            }

            var countryDistinct = new ObservableCollection<string>(countries.Distinct());

            return countryDistinct;
        }

        private List<Location> GetLocations()
        {
            var locations = new List<Location>();
            foreach (var request in _tourRequestService.GetAllRegularOnHold())
            {
                var location = new Location();
                location.City = request.City;
                location.Country = request.Country;
                if (!IsLocationAlreadyExists(locations, location))
                {
                    locations.Add(location);
                }
                
            }

            int locationId = 0;

            var locationEntities = new List<Location>();

            foreach (var location in locations)
            {
                location.Id = locationId;
                locationId++;
                locationEntities.Add(location);
            }

            return locationEntities;
        }

        private bool IsLocationAlreadyExists(List<Location> locations, Location location)
        {
            return locations.Any(l => l.City == location.City && l.Country == location.Country);
        }

        private void SearchTourRequests()
        {
            TourRequestCards = _tourRequestSearch.SearchTourRequests(City, Country, NumOfGuests, Language, MinDate, MaxDate);
        }

        private void ResetForm(object sender)
        {
            City = string.Empty;
            Country = string.Empty;
            Language = string.Empty;
            NumOfGuests = null;
            MinDate = null;
            MaxDate = null;
            Cities = GetCities();

            var tourRequestCardCreator = new TourRequestCardCreatorViewModel();
            TourRequestCards = tourRequestCardCreator.CreateRegularTourRequestCards();
        }

        private void ShowAcceptTourRequestForm(object sender)
        {
            SelectedTourRequestCard = sender as TourRequestCardViewModel;
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("AcceptTourRequestForm", App.LoggedUser);
        }

    }
}

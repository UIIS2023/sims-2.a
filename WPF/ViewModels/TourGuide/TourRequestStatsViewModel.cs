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
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class TourRequestStatsViewModel : ViewModel
    {
        private bool _isLanguageSelected;
        public bool IsLanguageSelected
        {
            get => _isLanguageSelected;
            set
            {
                if (_isLanguageSelected != value)
                {
                    _isLanguageSelected = value;
                    OnPropertyChanged("IsLanguageSelected");
                    if (_isLanguageSelected)
                    {
                        NumOfTourRequestsPerYears =
                            _searchForTourRequestStats.GetStatsByLanguage(SelectedLanguage);
                    }
                    
                }
            }
        }

        private bool _isLocationSelected;
        public bool IsLocationSelected
        {
            get => _isLocationSelected;
            set
            {
                if (_isLocationSelected != value)
                {
                    _isLocationSelected = value;
                    OnPropertyChanged("IsLocationSelected");
                    if (_isLocationSelected)
                    {
                        NumOfTourRequestsPerYears =
                            _searchForTourRequestStats.GetStatsByLocation(SelectedCity, SelectedCountry);
                    }
                    
                }
            }
        }

        private ObservableCollection<NumOfTourRequestsPerYearViewModel> _numOfTourRequestsPerYears;

        public ObservableCollection<NumOfTourRequestsPerYearViewModel> NumOfTourRequestsPerYears
        {
            get => _numOfTourRequestsPerYears;
            set
            {
                if (_numOfTourRequestsPerYears != value)
                {
                    _numOfTourRequestsPerYears = value;
                    OnPropertyChanged("NumOfTourRequestsPerYears");
                }
            }
        }

        private readonly TourRequestService _tourRequestService;
        private readonly StatsForTourProposalService _statsForTourProposalService;

        public string City { get; set; }

        public string Country { get; set; }

        public string Language { get; set; }

        public string Type { get; set; }

        private string _selectedCity;

        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    OnPropertyChanged("SelectedCity");
                    NumOfTourRequestsPerYears =
                        _searchForTourRequestStats.GetStatsByLocation(_selectedCity, _selectedCountry);
                }
            }
        }

        private string _selectedCountry;

        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    OnPropertyChanged("SelectedCountry");
                    NumOfTourRequestsPerYears =
                        _searchForTourRequestStats.GetStatsByLocation(_selectedCity, _selectedCountry);
                }
            }
        }

        private string _selectedLanguage;

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged("SelectedLanguage");
                    NumOfTourRequestsPerYears = _searchForTourRequestStats.GetStatsByLanguage(_selectedLanguage);
                }
            }
        }

        public ObservableCollection<string> Countries { get; set; }

        public ObservableCollection<string> Cities { get; set; }

        public List<Location> Locations { get; set; }

        public RelayCommand CreateSuggestedTourByLocationCommand { get; set; }

        public RelayCommand CreateSuggestedTourByLanguageCommand { get; set; }

        public RelayCommand CitySelectionChangedCommand { get; set; }
        public RelayCommand CountrySelectionChangedCommand { get; set; }

        public ObservableCollection<string> Languages { get; set; }

        private readonly SearchForTourRequestStats _searchForTourRequestStats;

        public RelayCommand ShowNumOfRequestsPerMonthCommand { get; set; }

        public TourRequestStatsViewModel()
        {
            _statsForTourProposalService = new StatsForTourProposalService();
            _tourRequestService = new TourRequestService();

            var mostRequiredLocation = _statsForTourProposalService.GetMostRequestedLocation();
            var mostRequiredLanguage = _statsForTourProposalService.GetMostRequiredLanguage();

            _searchForTourRequestStats = new SearchForTourRequestStats();
            

            City = mostRequiredLocation.City;
            Country = mostRequiredLocation.Country;
            Language = mostRequiredLanguage;

            Cities = GetCities();
            Countries = GetCountries();
            Locations = GetLocations();
            Languages = GetLanguages();

            _selectedLanguage = Languages.FirstOrDefault();
            _selectedCity = Cities.FirstOrDefault();
            _selectedCountry = Locations.Find(l => l.City == SelectedCity).Country;

            _isLanguageSelected = true;
            _isLocationSelected = false;

            CountrySelectionChangedCommand = new RelayCommand(ExecuteCountrySelectionChanged, CanExecuteMethod);
            CitySelectionChangedCommand = new RelayCommand(ExecuteCitySelectionChanged, CanExecuteMethod);
            CreateSuggestedTourByLocationCommand = new RelayCommand(CreateSuggestedTourByLocation, CanExecuteMethod);
            CreateSuggestedTourByLanguageCommand = new RelayCommand(CreateSuggestedTourByLanguage, CanExecuteMethod);
            ShowNumOfRequestsPerMonthCommand = new RelayCommand(ShowNumOfRequestsPerMonth, CanExecuteMethod);

            _numOfTourRequestsPerYears = _searchForTourRequestStats.GetStatsByLanguage(_selectedLanguage);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }


        //Initializing observable collections
        private ObservableCollection<string> GetCities()
        {
            var cities = new ObservableCollection<string>();
            foreach (var request in _tourRequestService.GetAll())
            {
                cities.Add(request.City);
            }

            var citiesDistinct = new ObservableCollection<string>(cities.Distinct());

            return citiesDistinct;
        }

        private ObservableCollection<string> GetCountries()
        {
            var countries = new ObservableCollection<string>();
            foreach (var request in _tourRequestService.GetAll())
            {
                countries.Add(request.Country);
            }

            var countryDistinct = new ObservableCollection<string>(countries.Distinct());

            return countryDistinct;
        }

        private List<Location> GetLocations()
        {
            var locations = new List<Location>();
            foreach (var request in _tourRequestService.GetAll())
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

        //Logic for selection changed

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

            var filteredLocations = Locations.Where(location => location.Country == SelectedCountry).ToList();

            foreach (var location in filteredLocations)
            {
                Cities.Add(location.City);
            }
        }

        public void CitySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedLocation = Locations.Find(location => location.City == SelectedCity);

            if (selectedLocation != null)
            {
                SelectedCountry = selectedLocation.Country;
                SelectedCity = selectedLocation.City;
            }
        }

        private ObservableCollection<string> GetLanguages()
        {
            var languages = new ObservableCollection<string>();

            foreach (var request in _tourRequestService.GetAll())
            {
                languages.Add(request.Language);
            }

            var languagesDistinct = new ObservableCollection<string>(languages.Distinct());

            return languagesDistinct;
        }

        private void ShowNumOfRequestsPerMonth(object sender)
        {
            var numOfReqPerYear = sender as NumOfTourRequestsPerYearViewModel;
            var windowPerMonth =
                new TourRequestsPerMonthWindow(numOfReqPerYear.NumOfRequestsPerMonths, numOfReqPerYear.Year);
            windowPerMonth.ShowDialog();
        }

        private void CreateSuggestedTourByLocation(object sender)
        {
            Type = "Location";
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("CreateSuggestedTour", App.LoggedUser);
        }
        
        private void CreateSuggestedTourByLanguage(object sender)
        {
            Type = "Language";
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("CreateSuggestedTour", App.LoggedUser);
        }

    }
}

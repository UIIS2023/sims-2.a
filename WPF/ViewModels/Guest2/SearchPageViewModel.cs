using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class SearchPageViewModel : ViewModel
    {
        private SearchPage _page;

        public ReadOnlyObservableCollection<string> Countries { get; set; }
        public ReadOnlyObservableCollection<string> Cities { get; set; }
        public ReadOnlyObservableCollection<string> Languages { get; set; }

        private LocationService _locationService;
        private TourService _tourService;

        private string _searchedLanguage;
        private string _searchedCity;
        private string _searchedCountry;
        private int _searchedOcupancy;
        private int _searchedDuration;

        public string SearchedLanguage
        {
            get => _searchedLanguage;
            set
            {
                if (value != _searchedLanguage)
                {
                    _searchedLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SearchedCity
        {
            get => _searchedCity;
            set
            {
                if (value != _searchedCity)
                {
                    _searchedCity = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SearchedCountry
        {
            get => _searchedCountry;
            set
            {
                if (value != _searchedCountry)
                {
                    _searchedCountry = value;
                    OnPropertyChanged();
                }
            }
        }
        public int SearchedOcupancy
        {
            get => _searchedOcupancy;
            set
            {
                if (value != _searchedOcupancy)
                {
                    _searchedOcupancy = value;
                    OnPropertyChanged();
                }
            }
        }
        public int SearchedDuration
        {
            get => _searchedDuration;
            set
            {
                if (value != _searchedDuration)
                {
                    _searchedDuration = value;
                    OnPropertyChanged();
                }
            }
        }
        public static ObservableCollection<TourViewModel> TourViewModels { get; set; }
        public static ObservableCollection<TourViewModel> SerbiaTourViewModels { get; set; }
        public static ObservableCollection<TourViewModel> SummerTourViewModels { get; set; }
        public static ArrowCommandsViewModel ArrowCommands { get; set; }
        public static User LoggedInUser { get; set; }
        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }

        private RelayCommand searchCommand;

        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
            set
            {
                searchCommand = value;
            }
        }

        public SearchPageViewModel(User loggedInUser, SearchPage page, ObservableCollection<TourViewModel> viewModels, ObservableCollection<TourViewModel> serbiaTourViewModels, ObservableCollection<TourViewModel> summerTourViewModels)
        {
            BackCommand = new RelayCommand(Execute_BackCommand, CanExecuteMethod);
            SearchCommand = new RelayCommand(Execute_SearchCommand, CanExecuteMethod);
            LoggedInUser = loggedInUser;
            _page = page;
            TourViewModels = new ObservableCollection<TourViewModel>();
            TourViewModels = viewModels;
            SerbiaTourViewModels = new ObservableCollection<TourViewModel>();
            SerbiaTourViewModels = serbiaTourViewModels;
            SummerTourViewModels = new ObservableCollection<TourViewModel>();
            SummerTourViewModels = summerTourViewModels;
            ArrowCommands = new ArrowCommandsViewModel(TourViewModels, SerbiaTourViewModels, SummerTourViewModels);
            _locationService = new LocationService();
            _tourService = new TourService();
            GetDataForComboBoxes();
        }

        private void GetDataForComboBoxes()
        {
            Countries = new ReadOnlyObservableCollection<string>(GetCountries());
            Cities = new ReadOnlyObservableCollection<string>(GetCities());
            Languages = new ReadOnlyObservableCollection<string>(GetLanguages());
        }

        private ObservableCollection<string> GetLanguages()
        {
            ObservableCollection<string> languages = new ObservableCollection<string>();
            foreach (Tour tour in _tourService.GetAll())
            {
                languages.Add(tour.Language);
            }
            languages = new ObservableCollection<string>(languages.Distinct());
            return languages;
        }

        private ObservableCollection<string> GetCities()
        {
            ObservableCollection<string> cities = new ObservableCollection<string>();
            foreach (Location location in _locationService.GetAll())
            {
                cities.Add(location.City);
            }
            cities = new ObservableCollection<string>(cities.Distinct());
            return cities;
        }

        private ObservableCollection<string> GetCountries()
        {
            ObservableCollection<string> coutries = new ObservableCollection<string>();
            foreach (Location location in _locationService.GetAll())
            {
                coutries.Add(location.Country);
            }
            coutries = new ObservableCollection<string>(coutries.Distinct());
            return coutries;
        }

        private void Execute_BackCommand(object obj)
        {
            ToursOverviewWindow window = new ToursOverviewWindow(LoggedInUser);
            window.Show();
            Window.GetWindow(_page).Close();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void Execute_SearchCommand(object sender)
        {
            try
            {
                LoadEnteredRequests();
                ObservableCollection<TourViewModel> searchResult = new ObservableCollection<TourViewModel>();

                foreach (var item in TourViewModels)
                {
                    bool isCorrect = IsApropriate(item);
                    if (isCorrect)
                    {
                        searchResult.Add(item);
                    }
                }
                ShowResults(searchResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Uneti podaci nisu u dobrom formatu: ", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

         private void LoadEnteredRequests()
         {
             SearchedLanguage = _page.language.Text;
             SearchedCity = _page.city.Text;
             SearchedCountry = _page.country.Text;
             string loadedDuration = _page.duration.Text;
             SearchedDuration = int.Parse(loadedDuration);
             string loadedOcupancy = _page.ocupancy.Text;
             SearchedOcupancy = int.Parse(loadedOcupancy);
         }
        
        private void ShowResults(ObservableCollection<TourViewModel> searchResult)
        {
            if (searchResult.Count > 0)
            {
                var navigationService = _page.SearchResultFrame.NavigationService;
                navigationService.Navigate(new SearchResultPage(searchResult));
            }
            else
            {
                MessageBox.Show("Nije pronadjena ni jedna tura spram unetih podataka. Pokusajte opet", "Pretraga", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private bool IsApropriate(TourViewModel tourViewModel)
        {
            bool checkCity = tourViewModel.City.ToLower().Contains(SearchedCity.ToLower()) || SearchedCity.Equals(string.Empty);
            bool checkCountry = tourViewModel.Country.ToLower().Contains(SearchedCountry.ToLower()) || SearchedCountry.Equals(string.Empty);
            bool checkLanguage = tourViewModel.Language.ToLower().Contains(SearchedLanguage.ToLower()) || SearchedLanguage.Equals(string.Empty);
            bool checkOcupancy = SearchedOcupancy <= tourViewModel.MaxNumOfGuests;
            bool checkDuration = SearchedDuration == tourViewModel.Duration || SearchedDuration == 0;

            return checkCity && checkCountry && checkLanguage && checkOcupancy && checkDuration;
        }

    }
}

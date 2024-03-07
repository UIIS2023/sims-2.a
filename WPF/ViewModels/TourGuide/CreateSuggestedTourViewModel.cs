using SOSTeam.TravelAgency.Application.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using System.Windows.Controls;
using SOSTeam.TravelAgency.WPF.Converters;
using static System.Net.Mime.MediaTypeNames;
using Image = SOSTeam.TravelAgency.Domain.Models.Image;
using SOSTeam.TravelAgency.Commands;
using System.Globalization;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class CreateSuggestedTourViewModel : ViewModel
    {

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

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
                }
            }
        }

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
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
                }
            }
        }

        private int _maxNumOfGuests;

        public int MaxNumOfGuests
        {
            get => _maxNumOfGuests;
            set
            {
                if (_maxNumOfGuests != value)
                {
                    _maxNumOfGuests = value;
                    OnPropertyChanged("MaxNumOfGuests");
                }
            }
        }

        private int _duration;

        public int Duration
        {
            get => _duration;

            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }

        private bool _canCreate;

        public bool CanCreate
        {
            get => _canCreate;
            set
            {
                if (_canCreate != value)
                {
                    _canCreate = value;
                    OnPropertyChanged("CanCreate");
                }
            }
        }





        private readonly LocationService _locationService;
        private readonly TourService _tourService;
        private readonly CheckpointService _checkpointService;
        private readonly AppointmentService _appointmentService;
        private readonly ImageService _imageService;
        private readonly NewTourNotificationService _newTourNotificationService;

        public bool CanSelectLocation { get; set; }
        public bool CanSelectLanguage { get; set; }

        public ObservableCollection<string> Countries { get; set; }

        public ObservableCollection<string> Cities { get; set; }

        public List<Location> Locations { get; set; }

        public ObservableCollection<string> Languages { get; set; } 

        public ObservableCollection<CheckpointCardViewModel> CheckpointCards { get; set; }
        public ObservableCollection<AppointmentCardViewModel> AppointmentCards { get; set; }

        public List<Image> Images { get; set; }


        public RelayCommand CitySelectionChangedCommand { get; set; }
        public RelayCommand CountrySelectionChangedCommand { get; set; }

        public RelayCommand ShowAddCheckpointsPageCommand { get; set; }
        public RelayCommand ShowAddAppointmentsPageCommand { get; set; }
        public RelayCommand ShowAddImagesCommand { get; set; }
        public RelayCommand ShowGalleryCommand { get; set; }
        public RelayCommand CreateTourClickCommand { get; set; }

        public CreateSuggestedTourViewModel(string city, string country, string language, string type)
        {
            _locationService = new LocationService();
            _tourService = new TourService();
            _checkpointService = new CheckpointService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
            _newTourNotificationService = new NewTourNotificationService();

            if (type == "Location")
            {
                _city = city;
                _country = country;
                Cities = new ObservableCollection<string> { city };
                Countries = new ObservableCollection<string> { country };
                Languages = new ObservableCollection<string>() { "English", "Germany", "Serbian", "Spanish" };
                CanSelectLocation = false;
                CanSelectLanguage = true;
            }
            else if (type == "Language")
            {
                _language = language;
                Locations = _locationService.GetAll();
                Countries = GetCountries();
                Cities = GetCities();
                Languages = new ObservableCollection<string> { language };
                CanSelectLocation = true;
                CanSelectLanguage = false;
            }

            CheckpointCards = new ObservableCollection<CheckpointCardViewModel>();
            AppointmentCards = new ObservableCollection<AppointmentCardViewModel>();
            Images = new List<Image>();

            CountrySelectionChangedCommand = new RelayCommand(ExecuteCountrySelectionChanged, CanExecuteMethod);
            CitySelectionChangedCommand = new RelayCommand(ExecuteCitySelectionChanged, CanExecuteMethod);
            ShowAddCheckpointsPageCommand = new RelayCommand(ShowAddCheckpointsPage, CanExecuteMethod);
            ShowAddAppointmentsPageCommand = new RelayCommand(ShowAddAppointmentsPage, CanExecuteMethod);
            ShowAddImagesCommand = new RelayCommand(ExecuteAddImages, CanExecuteMethod);
            ShowGalleryCommand = new RelayCommand(ShowImageGallery, CanExecuteMethod);
            CreateTourClickCommand = new RelayCommand(CreateTour, CanExecuteMethod);
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

            var filteredLocations = Locations.Where(location => location.Country == Country).ToList();

            foreach (var location in filteredLocations)
            {
                Cities.Add(location.City);
            }
        }

        public void CitySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedLocation = Locations.Find(location => location.City == City);

            if (selectedLocation != null)
            {
                Country = selectedLocation.Country;
                City = selectedLocation.City;
            }
        }

        private void ShowAddCheckpointsPage(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("AddCheckpoints", App.LoggedUser);
        }

        private void ShowAddAppointmentsPage(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("AddAppointments", App.LoggedUser);
        }

        private void ShowImageGallery(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("TourGallery", App.LoggedUser);
        }

        public ObservableCollection<string> GetCountries()
        {
            ObservableCollection<string> countries = new ObservableCollection<string>();
            foreach (Location location in Locations)
            {
                countries.Add(location.Country);
            }

            countries = new ObservableCollection<string>(countries.Distinct());
            return countries;
        }

        public ObservableCollection<string> GetCities()
        {
            ObservableCollection<string> citites = new ObservableCollection<string>();
            foreach (Location location in Locations)
            {
                citites.Add(location.City);
            }

            return citites;
        }


        private void CreateTour(object sender)
        {

            if (CheckpointsValidation() || AppointmentsValidation() || ImagesValidation())
            {
                if (CheckpointsValidation())
                {
                    App.TourGuideNavigationService.CreateOkMessageBox("You must have at least one START and one END checkpoint.");
                }

                if (AppointmentsValidation())
                {
                    App.TourGuideNavigationService.CreateOkMessageBox("You must have at least one appointment.");
                }

                if (ImagesValidation())
                {
                    App.TourGuideNavigationService.CreateOkMessageBox("You must have at least one image.");
                }
            }
            else
            {
                var tourId = _tourService.NextId();
                var tour = new Tour(tourId, Name, FindLocationId(), Description, Language, MaxNumOfGuests, Duration);

                _tourService.Save(tour);
                _checkpointService.SaveAll(TourEntitiesCreator.CreateCheckpoints(CheckpointCards, tourId));

                var lastAppointmentBeforeAdded = _appointmentService.GetAll().Count;

                _appointmentService.SaveAll(TourEntitiesCreator.CreateAppointments(AppointmentCards, tourId, App.LoggedUser.Id));
                SetImagesTourId(tourId);
                _imageService.SaveAll(Images);

                for (int i = lastAppointmentBeforeAdded+1; i <= _appointmentService.GetAll().Count; i++)
                {
                    _newTourNotificationService.CreateNotificationForAllUsers(i);
                }
                

                App.TourGuideNavigationService.AddPreviousPage();
                App.TourGuideNavigationService.SetMainFrame("HomePage", App.LoggedUser);
            }
        }



        private void SetImagesTourId(int tourId)
        {
            foreach (var image in Images)
            {
                image.EntityId = tourId;
            }
        }

        private int FindLocationId()
        {
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            string city = textInfo.ToTitleCase(City.ToLower());
            string country = textInfo.ToTitleCase(Country.ToLower());

            int locationId = GetLocationId(city, country);

            //Save new location
            if (locationId == -1)
            {
                var location = new Location(country, city);
                _locationService.Save(location);
            }

            var locations = _locationService.GetAll();
            locationId = locations[locations.Count - 1].Id;

            return locationId;
        }

        private int GetLocationId(string city, string country)
        {
            var location = _locationService.GetAll().Find(l => l.City == city && l.Country == country);
            if (location == null)
            {
                return -1;
            }
            else
            {
                return location.Id;
            }
        }

        private void ExecuteAddImages(object parameter)
        {
            var imagePaths = FileDialogService.GetImagePaths("Resources\\Images\\Tours", "/Resources/Images/Tours");
            Images = TourEntitiesCreator.CreateImages(imagePaths);
        }

        private bool CheckpointsValidation()
        {
            var containsStart = CheckpointCards.Any(c => c.Type == CheckpointType.START);
            var containsEnd = CheckpointCards.Any(c => c.Type == CheckpointType.END);

            return (!containsStart || !containsEnd);
        }

        private bool AppointmentsValidation()
        {
            return AppointmentCards.Count == 0;
        }

        private bool ImagesValidation()
        {
            return Images.Count == 0;


        }
    }
}

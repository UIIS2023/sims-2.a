using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Converters;
using SOSTeam.TravelAgency.WPF.ValidationRules.TourGuide;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;
using Image = SOSTeam.TravelAgency.Domain.Models.Image;



namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class CreateTourViewModel : ViewModel
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

        public ObservableCollection<string> Countries { get; set; }

        public ObservableCollection<string> Cities { get; set; }

        public User LoggedUser { get; set; }

        public List<Location> Locations { get; set; }

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

        public CreateTourViewModel(User loggedUser)
        {
            _locationService = new LocationService();
            _tourService = new TourService();
            _checkpointService = new CheckpointService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
            LoggedUser = loggedUser;

            _canCreate = false;
            Locations = _locationService.GetAll();
            Countries = GetCountries();
            Cities = GetCities();

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
            App.TourGuideNavigationService.SetMainFrame("AddCheckpoints", LoggedUser);
        }

        private void ShowAddAppointmentsPage(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("AddAppointments", LoggedUser);
        }

        private void ShowImageGallery(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("TourGallery", LoggedUser);
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
                _appointmentService.SaveAll(TourEntitiesCreator.CreateAppointments(AppointmentCards, tourId, LoggedUser.Id));
                SetImagesTourId(tourId);
                _imageService.SaveAll(Images);


                App.TourGuideNavigationService.AddPreviousPage();
                App.TourGuideNavigationService.SetMainFrame("HomePage", LoggedUser);
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
            var location = Locations.Find(l => l.Country == Country && l.City == City);
           
            return location.Id;
        }

        private void ExecuteAddImages(object parameter)
        {
            var imagePaths= FileDialogService.GetImagePaths("Resources\\Images\\Tours", "/Resources/Images/Tours");
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
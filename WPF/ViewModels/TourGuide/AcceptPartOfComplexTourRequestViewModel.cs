using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class AcceptPartOfComplexTourRequestViewModel: ViewModel
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
        private int? _duration;
        public int? Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged("Duration");
                    EnableSelectAppointment();
                    BlackoutDates = new ObservableCollection<DateTime>(CreateBlackoutDates());
                }
            }
        }

        private DateTime? _appointmentStart;

        public DateTime? AppointmentStart
        {
            get => _appointmentStart;
            set
            {
                if (_appointmentStart != value)
                {
                    _appointmentStart = value;
                    OnPropertyChanged("AppointmentStart");
                }
            }
        }

        private bool _canCancelAppointment;

        public bool CanSelectAppointment
        {
            get => _canCancelAppointment;
            set
            {
                if (_canCancelAppointment != value)
                {
                    _canCancelAppointment = value;
                    OnPropertyChanged("CanSelectAppointment");
                }
            }
        }

        private IEnumerable<DateTime> _blackoutDates;
        public IEnumerable<DateTime> BlackoutDates
        {
            get => _blackoutDates;
            set
            {
                if (!_blackoutDates.Equals(value))
                {
                    _blackoutDates = value;
                    OnPropertyChanged("BlackoutDates");
                }
            }
        }

        private DateTime _minDisplayDate;

        public DateTime MinDisplayDate
        {
            get => _minDisplayDate;
            set
            {
                if (_minDisplayDate != value)
                {
                    _minDisplayDate = value;
                    OnPropertyChanged("MinDisplayDate");
                }
            }
        }

        private DateTime _maxDisplayDate;

        public DateTime MaxDisplayDate
        {
            get => _maxDisplayDate;
            set
            {
                if (_maxDisplayDate != value)
                {
                    _maxDisplayDate = value;
                    OnPropertyChanged("MaxDisplayDate");
                }
            }
        }

        private readonly int _tourRequestId;

        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> Countries { get; set; }

        public ObservableCollection<string> Languages { get; set; }

        private readonly TourRequestService _tourRequestService;
        private readonly AppointmentScheduleService _appointmentScheduleService;
        private readonly TourRequest _selectedTourRequest;

        public ObservableCollection<CheckpointCardViewModel> CheckpointCards { get; set; }

        public List<Image> Images { get; set; }
        public int TourRequestId { get; set; }

        public RelayCommand ShowAddCheckpointsPageCommand { get; set; }
        public RelayCommand ShowAddImagesCommand { get; set; }
        public RelayCommand ShowGalleryCommand { get; set; }
        public RelayCommand AcceptTourClickCommand { get; set; }

        private readonly TourService _tourService;
        private readonly AppointmentService _appointmentService;
        private readonly ImageService _imageService;
        private readonly CheckpointService _checkpointService;
        private readonly LocationService _locationService;
        private readonly NewTourNotificationService _newTourNotificationService;
        private readonly ComplexTourRequestService _complexTourRequestService;

        public AcceptPartOfComplexTourRequestViewModel(int tourRequestId)
        {
            TourRequestId = tourRequestId;
            _tourService = new TourService();
            _appointmentService = new AppointmentService();
            _imageService = new ImageService();
            _checkpointService = new CheckpointService();
            _locationService = new LocationService();
            _newTourNotificationService = new NewTourNotificationService();
            _appointmentScheduleService = new AppointmentScheduleService(tourRequestId);
            _tourRequestService = new TourRequestService();
            _complexTourRequestService = new ComplexTourRequestService();

            Cities = new ObservableCollection<string>();
            Countries = new ObservableCollection<string>();
            Languages = new ObservableCollection<string>();
            CheckpointCards = new ObservableCollection<CheckpointCardViewModel>();
            Images = new List<Image>();
            _canCancelAppointment = false;
            _tourRequestId = tourRequestId;

            _blackoutDates = new ObservableCollection<DateTime>();


            var tourRequest = _tourRequestService.GetById(tourRequestId);
            _selectedTourRequest = tourRequest;

            _minDisplayDate = new DateTime(tourRequest.MaintenanceStartDate.Year, tourRequest.MaintenanceStartDate.Month, tourRequest.MaintenanceStartDate.Day);
            _maxDisplayDate = new DateTime(tourRequest.MaintenanceEndDate.Year, tourRequest.MaintenanceEndDate.Month, tourRequest.MaintenanceEndDate.Day);


            _duration = null;
            _appointmentStart = null;
            _name = string.Empty;
            _city = tourRequest.City;
            Cities.Add(tourRequest.City);
            _country = tourRequest.Country;
            Countries.Add(tourRequest.Country);
            _description = tourRequest.Description;
            _language = tourRequest.Language;
            Languages.Add(tourRequest.Language);
            _maxNumOfGuests = tourRequest.MaxNumOfGuests;

            ShowAddCheckpointsPageCommand = new RelayCommand(ShowAddCheckpointsPage, CanExecuteMethod);
            ShowAddImagesCommand = new RelayCommand(ExecuteAddImages, CanExecuteMethod);
            ShowGalleryCommand = new RelayCommand(ShowImageGallery, CanExecuteMethod);
            AcceptTourClickCommand = new RelayCommand(AcceptTour, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }


        private void EnableSelectAppointment()
        {
            if (Duration != null)
            {
                CanSelectAppointment = true;
            }
            else
            {
                CanSelectAppointment = false;
            }
        }

        private List<DateTime> CreateBlackoutDates()
        {
            var availableRanges = _appointmentScheduleService.CreateTourGuideSchedule(_tourRequestId, (int)Duration, App.LoggedUser.Id);
            var blackoutDates = new List<DateTime>();


            if (availableRanges.Count == 0)
            {
                var minDate = new DateTime(_selectedTourRequest.MaintenanceStartDate.Year, _selectedTourRequest.MaintenanceStartDate.Month, _selectedTourRequest.MaintenanceStartDate.Day);
                var maxDate = new DateTime(_selectedTourRequest.MaintenanceEndDate.Year, _selectedTourRequest.MaintenanceEndDate.Month, _selectedTourRequest.MaintenanceEndDate.Day);
                while (minDate <= maxDate)
                {
                    blackoutDates.Add(minDate);
                    minDate = minDate.AddDays(1);
                }
            }
            else
            {
                for (int i = 0; i < availableRanges.Count; i++)
                {
                    for (int j = i + 1; j < availableRanges.Count; j++)
                    {
                        var startDate = (DateTime)availableRanges[i].End;
                        var endDate = (DateTime)availableRanges[j].Start;
                        while (startDate < endDate)
                        {
                            blackoutDates.Add((DateTime)startDate);
                            startDate = startDate.AddDays(1);
                        }
                        break;
                    }
                }
            }

            var request = _tourRequestService.GetById(TourRequestId);

            var blackoutDateForComplexTour = _complexTourRequestService.GetBlackoutDates(request.ComplexTourRequestId);

            foreach (var blackoutDate in blackoutDateForComplexTour)
            {
                blackoutDates.Add(blackoutDate);
            }

            return blackoutDates;
        }

        private void ShowAddCheckpointsPage(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("AddCheckpoints", App.LoggedUser);
        }

        private void ShowImageGallery(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("TourGallery", App.LoggedUser);
        }

        private void AcceptTour(object sender)
        {

            if (!IsCheckpointsValid() || !IsImagesValid())
            {
                if (IsCheckpointsValid())
                {
                    App.TourGuideNavigationService.CreateOkMessageBox("You must have at least one START and one END checkpoint.");
                }


                if (IsImagesValid())
                {
                    App.TourGuideNavigationService.CreateOkMessageBox("You must have at least one image.");
                }
            }
            else
            {

                var tourId = _tourService.NextId();

                Appointment appointment = new Appointment
                {
                    Start = (DateTime)AppointmentStart,
                    Started = false,
                    Finished = false,
                    Occupancy = _selectedTourRequest.MaxNumOfGuests,
                    TourId = tourId,
                    UserId = App.LoggedUser.Id
                };

                var tour = new Tour(tourId, Name, FindLocationId(), Description, Language, MaxNumOfGuests, (int)Duration);

                _tourService.Save(tour);
                _checkpointService.SaveAll(TourEntitiesCreator.CreateCheckpoints(CheckpointCards, tourId));
                _appointmentService.Save(appointment);
                var appointments = _appointmentService.GetAllByUserId(App.LoggedUser.Id);
                int appointmentId = appointments[appointments.Count - 1].Id;
                SetImagesTourId(tourId);
                _imageService.SaveAll(Images);

                _selectedTourRequest.Status = StatusType.ACCEPTED;
                _tourRequestService.Update(_selectedTourRequest);
                _newTourNotificationService.CreateNotificationForUser(appointmentId, _selectedTourRequest.UserId, TourRequestId);

                App.TourGuideNavigationService.AddPreviousPage();
                App.TourGuideNavigationService.SetMainFrame("HomePage", App.LoggedUser);
            }
        }


        //Find or create location id
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

            return location.Id;

        }

        private void SetImagesTourId(int tourId)
        {
            foreach (var image in Images)
            {
                image.EntityId = tourId;
            }
        }


        private void ExecuteAddImages(object parameter)
        {
            var imagePaths = FileDialogService.GetImagePaths("Resources\\Images\\Tours", "/Resources/Images/Tours");
            Images = TourEntitiesCreator.CreateImages(imagePaths);
        }

        private bool IsCheckpointsValid()
        {
            var containsStart = CheckpointCards.Any(c => c.Type == CheckpointType.START);
            var containsEnd = CheckpointCards.Any(c => c.Type == CheckpointType.END);

            return !(!containsStart || !containsEnd);
        }

        private bool IsImagesValid()
        {
            return !(Images.Count == 0);
        }

    }
}

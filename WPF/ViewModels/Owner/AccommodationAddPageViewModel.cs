using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    internal class AccommodationAddPageViewModel : ViewModel
    {
        
        private AccommodationService _accommodationService;
        private LocationService _locationService;
        private ImageService _imageService;

        public User LoggedInUser { get; private set; }
        public string AName { get; set; }
        public Accommodation.AccommodationType SelectedType { get; set; }
        public string LocationId { get; set; }
        public int MaxGuests { get; set; }
        public int MinDaysStay { get; set; }
        public int MinDaysForCancelation { get; set; }
        public List<Location> Locations { get; set; }
        public ReadOnlyObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }


        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    CountryComboBoxSelectionChanged();
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
                }
            }
        }

        private ObservableCollection<Domain.Models.Image> _selectedImages;
        public ObservableCollection<Domain.Models.Image> SelectedImages
        {
            get { return _selectedImages; }
            set
            {
                _selectedImages = value;
                OnPropertyChanged("SelectedImages");
            }
        }
        public Domain.Models.Image SelectedImage { get; set; }

        public bool CountryBoxEnabled { get; set; }
        public RelayCommand Cancel { get; private set; }
        public RelayCommand AddAccommodation { get; private set; }
        public RelayCommand AddPicture { get; private set; }
        public RelayCommand RemovePicture { get; private set; }
        public RelayCommand SetCover { get; private set; }
        public AccommodationAddPageViewModel()
        {
            LoggedInUser = App.LoggedUser;
            _accommodationService = new AccommodationService();
            _locationService = new();
            _imageService = new();


            AddAccommodation = new RelayCommand(Execute_AddAccommodation, CanExecuteAddAccommodation);
            Cancel = new RelayCommand(Execute_Cancel, CanExecuteCancel);
            AddPicture = new RelayCommand(Execute_AddPicture, CanExecuteCancel);
            RemovePicture = new RelayCommand(Execute_RemovePicture, CanExecuteSelectedItem);
            SetCover = new RelayCommand(Execute_SetCover, CanExecuteSelectedItem);

            _selectedImages = new();
            Locations = new List<Location>(_locationService.GetAll());
            Countries = new ReadOnlyObservableCollection<string>(GetCountries());
            Cities = new ObservableCollection<string>();
            SelectedType = Accommodation.AccommodationType.HUT;
            CountryBoxEnabled = true;

            AName = string.Empty;
            Country = string.Empty;
            City = string.Empty;
            MinDaysForCancelation = 1;
            MinDaysStay = 1;
            MaxGuests = 1;
        }

        private void Execute_SetCover(object obj)
        {
            foreach (var item in _selectedImages.Where(t => t.Cover == true)) 
            {
                item.Cover = false;
            }
            SelectedImage.Cover = true;
        }

        private bool CanExecuteSelectedItem(object obj)
        {
            return SelectedImage != null;
        }

        private void Execute_RemovePicture(object obj)
        {
            SelectedImages.Remove(SelectedImage);
        }

        private void Execute_AddPicture(object obj)
        {
            var tempImagePaths = FileDialogService.GetImagePaths("Resources\\Images\\Owner", "Resources/Images/Owner");
            foreach (var imagePath in tempImagePaths) 
            {
                var image = new Domain.Models.Image("/" + imagePath, false, -1, ImageType.ACCOMMODATION);
                SelectedImages.Add(image);
            }

            

            

        }

        private bool CanExecuteAddAccommodation(object obj)
        {
            return IsValid();
        }

        private bool CanExecuteCancel(object obj)
        {
            return true;
        }


        private void Execute_AddAccommodation(object obj)
        {
            int locationId = FindLocationId();
            Accommodation accommodation = new Accommodation(AName, SelectedType, locationId, MaxGuests, MinDaysStay, LoggedInUser.Id, MinDaysForCancelation);
            var id = _accommodationService.SaveAndReturn(accommodation).Id;
            SaveImages(id);
            App.OwnerNavigationService.NavigateMainWindow("Accommodation");
            return;
        }

        private void SaveImages(int accommodationId)
        {
            if (SelectedImages.Count == 0) return;
            if (!SelectedImages.Any(t => t.Cover)) SelectedImages[0].Cover = true;
            foreach(var img in _selectedImages) 
            {
                img.EntityId = accommodationId;
                _imageService.Save(img);
            }
        }

        private void Execute_Cancel(object obj)
        {
            App.OwnerNavigationService.NavigateMainWindow("Accommodation");
            return;
        }


        

        private void CountryComboBoxSelectionChanged()
        {
            City = "";
            Cities.Clear();
            GetCities();
        }



        public ObservableCollection<string> GetCountries()
        {
            ObservableCollection<string> coutries = new ObservableCollection<string>();
            foreach (Location location in Locations)
            {
                coutries.Add(location.Country);
            }
            coutries = new ObservableCollection<string>(coutries.Distinct());
            return coutries;
        }


        public void GetCities()
        {
            foreach (Location location in Locations)
            {
                if (location.Country.Equals(Country))
                {
                    Cities.Add(location.City);
                }
            }
        }

        private int FindLocationId()
        {
            List<Location> locations = new List<Location>(_locationService.GetAll());
            Location location = locations.Find(l => l.Country.Equals(Country) && l.City.Equals(City));
            return location.Id;
        }

        private bool IsValid()
        {
            if(City == null || Country == null)  return false;
            if (MinDaysForCancelation < 0 || MinDaysStay < 0 || MaxGuests < 0) return false;
            if (AName.Length < 1 || City.Length < 1 || Country.Length < 1 || SelectedImages.Count < 1) return false;
            return true;
        }
    }

    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Accommodation.AccommodationType enumValue = (Accommodation.AccommodationType)value;
            Accommodation.AccommodationType targetValue = (Accommodation.AccommodationType)parameter;
            return enumValue == targetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            Accommodation.AccommodationType targetValue = (Accommodation.AccommodationType)parameter;
            return isChecked ? targetValue : Binding.DoNothing;
        }
    }
}

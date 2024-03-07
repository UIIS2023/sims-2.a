using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class ForumPageViewModel : ViewModel
    {
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
                    SearchForums();
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
                    SearchForums();
                }

            }
        }
        public List<Location> Locations { get; set; }
        public ReadOnlyObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }

        public bool CountryBoxEnabled { get; set; }

        public ForumViewModel SelectedForum { get; set; }

        private ObservableCollection<ForumViewModel> _forums;
        public ObservableCollection<ForumViewModel> Forums
        {
            get { return _forums; }
            set
            {
                _forums = value;
                OnPropertyChanged("Forums");
            }
        }
        public RelayCommand OpenForum { get; private set; }

        private List<ForumViewModel> _forumList;
        private UserService _userService;
        private LocationService _locationService;
        private ForumService _forumService;
        private AccommodationService _accommodationService;
        public ForumPageViewModel()
        {
            _userService = new();
            _locationService = new();
            _forumService = new();
            _accommodationService = new();

            Locations = new List<Location>(_locationService.GetAll());
            Countries = new ReadOnlyObservableCollection<string>(GetCountries());
            Cities = new ObservableCollection<string>();
            CountryBoxEnabled = true;
            OpenForum = new RelayCommand(Execute_OpenForum, CanExecuteOpenForum);


            Forums = new();
            _forumList = new();
            GetForums();
        }

        private void Execute_OpenForum(object obj)
        {
            App.OwnerNavigationService.SetPage(new ForumInfoPage(_forumService.GetById(SelectedForum.Id)));
        }

        private bool CanExecuteOpenForum(object obj)
        {
            return SelectedForum != null;
        }

        private void SearchForums() 
        {
            Forums.Clear();
            foreach (var forum in _forumList) 
            {
                if ( !(City == null || City.Equals("") || City.Equals(forum.City) ) ) 
                {
                    continue;
                }
                
                if ( !(Country == null || Country.Equals("") || Country.Equals(forum.Country) ) ) 
                {
                    continue;
                }
                Forums.Add(forum);
            }
        }
        private void GetForums()
        {
            // PROMENI NA KRAJU
            var accommodations = _accommodationService.GetAllByUserId(App.LoggedUser.Id);
            var forums = _forumService.GetAll().Where(f => accommodations.Any(a => a.LocationId == f.LocationId));
            foreach (var forum in forums)
            {
                string guestUsername = _userService.GetById(forum.UserId).Username;
                string title = forum.Title;
                var location = _locationService.GetById(forum.LocationId);
                string locationName = _locationService.GetFullName(location);

                _forumList.Add(new ForumViewModel(forum.Id, guestUsername, title, locationName, location.Country, location.City));
            }
            Forums.AddRange(_forumList);
        }

        private void CountryComboBoxSelectionChanged()
        {
            City ="";
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
            coutries.Add("");
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
    }

    public class ForumViewModel
    {
        public int Id { get; set; }
        public string GuestUsername { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public ForumViewModel(int id, string guestUsername, string title, string location,string country,string city)
        {
            Id = id;
            GuestUsername = guestUsername;
            Title = title;
            Location = location;
            Country = country;
            City = city;
        }
    }
}

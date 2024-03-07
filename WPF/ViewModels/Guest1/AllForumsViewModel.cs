using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class AllForumsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public User LoggedInUser { get; set; }
        public NavigationService NavigationService { get; set; }
        private string selectedLocation { get; set; }
        public string SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                selectedLocation = value;
                OnPropertyChaged("SelectedLocation");
            }
        }
        private string startQuestion { get; set; }
        public string StartQuestion
        {
            get { return startQuestion; }
            set
            {
                startQuestion = value;
                OnPropertyChaged("StartQuestion");
            }
        }
        public List<string> _locations { get; set; }
        public Dictionary<int, string> Locations { get; set; }
        public ObservableCollection<Forum> AllForums { get; set; }
        public ObservableCollection<Forum> MyForums { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand GoToForum { get; set; }
        public RelayCommand OpenForumCommand { get; set; }
        private NotificationService _notificationService;

        public AllForumsViewModel(User user, NavigationService service)
        {
            LoggedInUser = user;
            NavigationService = service;
            _notificationService = new();
            ForumService forumService = new ForumService();
            MyForums = new ObservableCollection<Forum>();
            AllForums = new ObservableCollection<Forum>(forumService.GetAll());
            _locations = new List<string>();
            Locations = new Dictionary<int, string>();
            StartQuestion = string.Empty;

            FindMyForums();
            FillCombobox();

            GoBackCommand = new RelayCommand(Execute_GoBack);
            GoToForum = new RelayCommand(Execute_GoToForum);
            OpenForumCommand = new RelayCommand(Execute_OpenForum);
        }

        private void FindMyForums()
        {
            foreach (Forum forum in AllForums)
            {
                if(forum.UserId == LoggedInUser.Id)
                {
                    MyForums.Add(forum);
                }
            }
        }

        private void FillCombobox()
        {
            LocationService locationService = new LocationService();
            foreach(var location in locationService.GetAll())
            {
                if(!IsExists(location))
                {
                    string fullName = location.City + ", " + location.Country;
                    _locations.Add(fullName);
                    Locations.Add(location.Id, fullName);
                }
            }
            _locations.Add("");
            Locations.Add(-1, "");
        }

        private bool IsExists(Location location)
        {
            ForumService forumService = new ForumService();
            foreach(var item in forumService.GetAll())
            {
                if (item.LocationId == location.Id)
                    return true;
            }

            return false;
        }

        protected void OnPropertyChaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Execute_GoBack(object sender)
        {
            NavigationService.GoBack();
        }

        private void Execute_GoToForum(object sender)
        {
            Forum? selected = sender as Forum;
            NavigationService.Navigate(new OneForumPage(LoggedInUser, NavigationService, selected));
        }

        private void Execute_OpenForum(object sender)
        {
            if(Validate())
            {
                Forum forum = new Forum(LoggedInUser.Id, FindLocation(), SelectedLocation, StartQuestion, true);
                ForumService forumService = new ForumService();
                forumService.Save(forum);
                _notificationService.AddForumNotifications(forum);
                MessageBox.Show("Uspješno ste kreirali novi forum.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                MyForums.Add(forum);
                AllForums.Add(forum);
                SelectedLocation = _locations[_locations.Count-1];
                StartQuestion = null;
            }
        }

        private int FindLocation()
        {
            foreach(var item in Locations)
            {
                if(item.Value.Equals(SelectedLocation))
                {
                    return item.Key;
                }
            }

            return -1;
        }

        private bool Validate()
        {
            if (SelectedLocation == null || SelectedLocation.Equals(""))
            {
                MessageBox.Show("Niste odabrali lokaciju za koju želite pokrenuti forum", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (StartQuestion == null || StartQuestion.Equals(""))
            {
                MessageBox.Show("Niste ostavili početni komentar.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}

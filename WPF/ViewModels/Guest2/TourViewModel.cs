using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class TourViewModel
    {
        public User LoggedInUser { get; set; }

        public int TourId;
        public string Name { get; set; }
        public string Language { get; set; }
        public int Duration { get; set; }
        public int MaxNumOfGuests { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string LocationFullName { get; set; }
        public string ImagePath { get; set; }

        private readonly TourService _tourService;
        private readonly AppointmentService _appoitmentService;

        private RelayCommand _tourDetailsCommand;

        public RelayCommand TourDetailsCommand
        {
            get { return _tourDetailsCommand; }
            set
            {
                _tourDetailsCommand = value;
            }
        }

        public TourViewModel(int id,string name,string language,int duration,int maxNumOfGuests,string city,string country,User loggedInUser,string path) 
        {
            TourId = id;
            Name = name;
            Language = language;
            Duration = duration;
            MaxNumOfGuests= maxNumOfGuests;
            Country = country;
            City = city;
            ImagePath = path;
            LocationFullName = city + " (" + country + ")";
            LoggedInUser = loggedInUser;
            _tourService = new TourService();
            _appoitmentService = new AppointmentService();
            TourDetailsCommand = new RelayCommand(Execute_OpenBookTourWindow, CanExecuteMethod);
        }

        private void Execute_OpenBookTourWindow(object obj)
        {
            Tour tour = _tourService.FindTourById(TourId);
            var currentApp = System.Windows.Application.Current;

            foreach (Window window in currentApp.Windows)
            {
                if (window is ToursOverviewWindow)
                {
                    if (!_appoitmentService.CheckAvailableAppointments(tour))
                    {
                        MessageBox.Show("Nema slobodnih mesta za odabranu turu, pogledajte druge ture na istoj lokaciji.");
                        var navigationService = ((ToursOverviewWindow)window).AlternativeFrame.NavigationService;
                        navigationService.Navigate(new AlternativeToursPage(tour, LoggedInUser, ((ToursOverviewWindow)window)));
                    }
                    else
                    {
                        BookTourWindow bookWindow = new BookTourWindow(TourId, LoggedInUser);
                        bookWindow.ShowDialog();
                    }
                }
            }
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}

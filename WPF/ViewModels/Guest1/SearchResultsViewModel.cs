using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class SearchResultsViewModel
    {
        public User LoggedInUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public ObservableCollection<LocAccommodationDTO> accommodationDTOs { get; set; }
        public LocAccommodationDTO SelectedAccommodationDTO { get; set; }
        public RelayCommand reserveCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }

        public SearchResultsViewModel(List<LocAccommodationDTO> Results, User user, NavigationService service)
        {
            accommodationDTOs = new ObservableCollection<LocAccommodationDTO>(Results);
            LoggedInUser = user;
            reserveCommand = new RelayCommand(ExecuteReserveAccommodation);
            GoBackCommand = new RelayCommand(Execute_GoBack);
            NavigationService = service;
        }

        public void ExecuteReserveAccommodation(object sender)
        {
            if (SelectedAccommodationDTO != null)
            {
                NavigationService.Navigate(new EnterReservationPage(SelectedAccommodationDTO, LoggedInUser, false, NavigationService));
            }
            else
            {
                MessageBox.Show("Morate da odaberete smeštaj za rezervaciju.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Execute_GoBack(object sender)
        {
            NavigationService.GoBack();
        }
    }
}
